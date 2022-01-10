using System;
using System.IO.Abstractions;
using System.Reactive;
using System.Reactive.Linq;
using OBSWebsocketDotNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Admin.Setup;
using Slingo.Admin.Word;
using Slingo.Game;
using Slingo.Game.Word;
using Slingo.Sound;
using SlingoLib.Serialization;
using Splat;

namespace Slingo
{
    public class AdminViewModel : ReactiveObject
    {
        private GameState _state;

        [Reactive] public ReactiveObject SelectedAdminViewModel { get; private set; }
        
        [Reactive] public GameViewModel GameViewModel { get; private set; }
        
        public AdminViewModel()
        {
            var correctSound = new CachedSound(@"Resources\Sounds\WordGame\correct.wav");
            
;           var setupViewModel = new SetupViewModel();
            setupViewModel.Start.Subscribe(settings =>
            {
                var audioPlaybackEngine = new AudioPlaybackEngine(settings.AudioDevice,correctSound.WaveFormat.SampleRate, correctSound.WaveFormat.Channels);
                ActiveSceneContainer activeSceneContainer = null;

                if (settings.ObsSettings != null)
                {
                    var obsWebsocket = new OBSWebsocket();
                    obsWebsocket.Connect(settings.ObsSettings.ObsAddress, settings.ObsSettings.ObsPassword);

                    if (obsWebsocket.IsConnected)
                    {
                        activeSceneContainer = new ActiveSceneContainer(obsWebsocket);
                    }
                }

                
                _state = new GameState(new TeamState(settings.StartingTeamIndex == 0), new TeamState(settings.StartingTeamIndex == 1));

                GameViewModel = new GameViewModel(settings, _state, audioPlaybackEngine, activeSceneContainer);
                var inputViewModel = new InputViewModel(_state,new WordRepository(new FileSystem(), @"Resources\5letterwoorden.txt"), settings, GameViewModel.WordGameViewModel);
                inputViewModel.FocusTeam1.Subscribe(x => GameViewModel.FocusTeam(0));
                inputViewModel.FocusTeam2.Subscribe(x => GameViewModel.FocusTeam(1));
                inputViewModel.FocusBingoCard.Subscribe(x => GameViewModel.Team1ViewModel.FocusBingoCard());
                inputViewModel.FocusBingoCard.Subscribe(x => GameViewModel.Team2ViewModel.FocusBingoCard());
                inputViewModel.FocusWordGame.Subscribe(x => GameViewModel.Team1ViewModel.FocusWordGame());
                inputViewModel.FocusWordGame.Subscribe(x => GameViewModel.Team2ViewModel.FocusWordGame());
                
                // Bingo input
                SubscribeToBingoInput(inputViewModel);

                SelectedAdminViewModel = inputViewModel;

                var view = Locator.Current.GetService<IViewFor<GameViewModel>>();
                var window = view as ReactiveWindow<GameViewModel>;
                window.ViewModel = GameViewModel;
                window.Show();
            });
            SelectedAdminViewModel = setupViewModel;
        }

        private void SubscribeToBingoInput(InputViewModel viewmodel)
        {
            // Team 1
            viewmodel.BingoSetupViewModel1.Initialize.Subscribe(async x =>
            {
                await GameViewModel.Team1ViewModel.BingoViewModel.FillInitialBalls();
                viewmodel.BingoSetupViewModel1.State = BingoCardState.Filled;
            });

            viewmodel.BingoSetupViewModel1.ClearBalls.Subscribe(async x =>
            {
                await GameViewModel.Team1ViewModel.BingoViewModel.ClearBalls();
                GameViewModel.Team1ViewModel.CreateNewBingoCard();
                viewmodel.BingoSetupViewModel1.State = BingoCardState.Empty;
            });

            viewmodel.BingoSetupViewModel1.BallSubmitted.Subscribe(async x =>
            {
                if (!x.add)
                {
                    GameViewModel.Team1ViewModel.BingoViewModel.ClearBall(x.number);
                    return;
                }

                if (await GameViewModel.Team1ViewModel.BingoViewModel.FillBall(x.number))
                {
                    viewmodel.BingoSetupViewModel1.State = BingoCardState.Won;
                    _state.Team1.Score += 100;
                    _state.SwitchActiveTeam();
                }
            });

            viewmodel.BingoSetupViewModel1.GreenBallSubmitted.Subscribe(x =>
            {
                if (x)
                {
                    GameViewModel.Team1ViewModel.AddGreenBall();
                }
                else
                {
                    GameViewModel.Team1ViewModel.RemoveGreenBall();
                }

            });

            viewmodel.BingoSetupViewModel1.RedBallSubmitted.Subscribe(x =>
            {
                _state.SwitchActiveTeam();
            });


            // Team 2
            viewmodel.BingoSetupViewModel2.Initialize.Subscribe(async x =>
            {
                await GameViewModel.Team2ViewModel.BingoViewModel.FillInitialBalls();
                viewmodel.BingoSetupViewModel2.State = BingoCardState.Filled;
            });

            viewmodel.BingoSetupViewModel2.ClearBalls.Subscribe(async x =>
            {
                await GameViewModel.Team2ViewModel.BingoViewModel.ClearBalls();
                GameViewModel.Team2ViewModel.CreateNewBingoCard();
                viewmodel.BingoSetupViewModel2.State = BingoCardState.Empty;
            });

            viewmodel.BingoSetupViewModel2.BallSubmitted.Subscribe(async x =>
            {
                if (!x.add)
                {
                    GameViewModel.Team2ViewModel.BingoViewModel.ClearBall(x.number);
                    return;
                }

                if (await GameViewModel.Team2ViewModel.BingoViewModel.FillBall(x.number))
                {
                    viewmodel.BingoSetupViewModel2.State = BingoCardState.Won;
                    _state.Team2.Score += 100;
                    _state.SwitchActiveTeam();
                }
            });

            viewmodel.BingoSetupViewModel2.GreenBallSubmitted.Subscribe(x =>
            {
                if (x)
                {
                    GameViewModel.Team2ViewModel.AddGreenBall();
                }
                else
                {
                    GameViewModel.Team2ViewModel.RemoveGreenBall();
                }
            });

            viewmodel.BingoSetupViewModel2.RedBallSubmitted.Subscribe(x =>
            {
                _state.SwitchActiveTeam();
            });

        }
    }
}
