using System;
using System.IO.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Admin.Setup;
using Slingo.Admin.Word;
using Slingo.Game.Word;
using Slingo.Sound;
using SlingoLib.Serialization;
using Splat;

namespace Slingo
{
    public class AdminViewModel : ReactiveObject
    {
        private GameState _state;
        private SetupViewModel _setupViewModel;
        private InputViewModel _inputViewModel;
        private AudioPlaybackEngine _audioPlaybackEngine;

        [Reactive] public ReactiveObject SelectedAdminViewModel { get; private set; }
        
        [Reactive] public GameViewModel GameViewModel { get; private set; }
        
        public AdminViewModel()
        {
            var correctSound = new CachedSound(@"Resources\Sounds\WordGame\correct.wav");
            _audioPlaybackEngine = new AudioPlaybackEngine(correctSound.WaveFormat.SampleRate, correctSound.WaveFormat.Channels);
;            _setupViewModel = new SetupViewModel();
            _setupViewModel.Start.Subscribe(settings =>
            {

                _state = new GameState(new TeamState(settings.StartingTeamIndex == 0), new TeamState(settings.StartingTeamIndex == 1));

                GameViewModel = new GameViewModel(settings, _state, _audioPlaybackEngine);
                _inputViewModel = new InputViewModel(_state,new WordRepository(new FileSystem(), @"Resources\basiswoorden-gekeurd.txt"), settings, GameViewModel.WordGameViewModel);
                _inputViewModel.FocusTeam1.Subscribe(x => GameViewModel.FocusTeam(0));
                _inputViewModel.FocusTeam2.Subscribe(x => GameViewModel.FocusTeam(1));
                _inputViewModel.FocusBingoCard.Subscribe(x => GameViewModel.Team1ViewModel.FocusBingoCard());
                _inputViewModel.FocusBingoCard.Subscribe(x => GameViewModel.Team2ViewModel.FocusBingoCard());
                _inputViewModel.FocusWordGame.Subscribe(x => GameViewModel.Team1ViewModel.FocusWordGame());
                _inputViewModel.FocusWordGame.Subscribe(x => GameViewModel.Team2ViewModel.FocusWordGame());
                
                // Bingo input
                SubscribeToBingoInput(_inputViewModel);

                SelectedAdminViewModel = _inputViewModel;

                var view = Locator.Current.GetService<IViewFor<GameViewModel>>();
                var window = view as ReactiveWindow<GameViewModel>;
                window.ViewModel = GameViewModel;
                window.Show();
            });
            SelectedAdminViewModel = _setupViewModel;
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
                if (await GameViewModel.Team1ViewModel.BingoViewModel.FillBall(x))
                {
                    viewmodel.BingoSetupViewModel1.State = BingoCardState.Won;
                    _state.Team1.Score += 100;
                    _state.SwitchActiveTeam();
                    
                }
            });

            viewmodel.BingoSetupViewModel1.GreenBallSubmitted.Subscribe(x =>
            {
                GameViewModel.Team1ViewModel.AddGreenBall();
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
                if (await GameViewModel.Team2ViewModel.BingoViewModel.FillBall(x))
                {
                    viewmodel.BingoSetupViewModel2.State = BingoCardState.Won;
                    _state.Team2.Score += 100;
                    _state.SwitchActiveTeam();
                }
            });

            viewmodel.BingoSetupViewModel2.GreenBallSubmitted.Subscribe(x =>
            {
                GameViewModel.Team2ViewModel.AddGreenBall();
            });

            viewmodel.BingoSetupViewModel2.RedBallSubmitted.Subscribe(x =>
            {
                _state.SwitchActiveTeam();
            });

        }
    }
}
