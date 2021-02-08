using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Admin.Setup;
using Slingo.Admin.Word;
using Slingo.Game;
using Slingo.Game.Bingo;
using Slingo.Game.Word;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic.Word;
using SlingoLib.Serialization;
using Splat;

namespace Slingo.Admin
{
    public class AdminViewModel : ReactiveObject
    {
        private SetupViewModel _setupViewModel;
        private GameWindowViewModel _gameWindowViewModel;
        private AudioPlaybackEngine _audioPlaybackEngine;

        [Reactive] public ReactiveObject SelectedViewModel { get; private set; }
        
        public AdminViewModel()
        {
            var correctSound = new CachedSound(@"Resources\Sounds\WordGame\correct.wav");
            _audioPlaybackEngine = new AudioPlaybackEngine(correctSound.WaveFormat.SampleRate, correctSound.WaveFormat.Channels);
;            _setupViewModel = new SetupViewModel();
            _setupViewModel.Start.Subscribe(settings =>
            {
                Team team1 = new Team(settings.Team1);
                Team team2 = new Team(settings.Team2);

                _gameWindowViewModel.StartGame(new GameViewModel(settings, team1, team2, _audioPlaybackEngine));
                
                InputViewModel inputViewModel = new InputViewModel(new WordRepository(new FileSystem(), @"Resources\basiswoorden-gekeurd.txt"), settings, _audioPlaybackEngine);
                inputViewModel.FocusTeam1.Subscribe(x => _gameWindowViewModel.GameViewModel.FocusTeam(0));
                inputViewModel.FocusTeam2.Subscribe(x => _gameWindowViewModel.GameViewModel.FocusTeam(1));
                inputViewModel.FocusBingoCard.Subscribe(x => _gameWindowViewModel.GameViewModel.Team1ViewModel.FocusBingoCard());
                inputViewModel.FocusBingoCard.Subscribe(x => _gameWindowViewModel.GameViewModel.Team2ViewModel.FocusBingoCard());
                inputViewModel.FocusWordGame.Subscribe(x => _gameWindowViewModel.GameViewModel.Team1ViewModel.FocusWordGame());
                inputViewModel.FocusWordGame.Subscribe(x => _gameWindowViewModel.GameViewModel.Team2ViewModel.FocusWordGame());
                
                // Word input
                inputViewModel.WhenAnyValue(x => x.WordInputViewModel).Where(x=>x!=null).Subscribe(SubscribeToWordInput);

                // Bingo input
                SubscribeToBingoInput(inputViewModel);
                
                SelectedViewModel = inputViewModel;
            });

            SelectedViewModel = _setupViewModel;

            _gameWindowViewModel = new GameWindowViewModel();
            var view = Locator.Current.GetService<IViewFor<GameWindowViewModel>>();
            var window = view as ReactiveWindow<GameWindowViewModel>;
            window.ViewModel = _gameWindowViewModel;
            window.Show();
        }

        private void SubscribeToBingoInput(InputViewModel viewmodel)
        {
            // Team 1
            viewmodel.BingoSetupViewModel1.Initialize.Subscribe(async x =>
            {
                await _gameWindowViewModel.GameViewModel.Team1ViewModel.BingoViewModel.FillInitialBalls();
                viewmodel.BingoSetupViewModel1.State = BingoCardState.Filled;
            });

            viewmodel.BingoSetupViewModel1.ClearBalls.Subscribe(async x =>
            {
                await _gameWindowViewModel.GameViewModel.Team1ViewModel.BingoViewModel.ClearBalls();
                _gameWindowViewModel.GameViewModel.Team1ViewModel.CreateNewBingoCard();
                viewmodel.BingoSetupViewModel1.State = BingoCardState.Empty;
            });

            viewmodel.BingoSetupViewModel1.BallSubmitted.Subscribe(async x =>
            {
                if (await _gameWindowViewModel.GameViewModel.Team1ViewModel.BingoViewModel.FillBall(x))
                {
                    viewmodel.BingoSetupViewModel1.State = BingoCardState.Won;
                }
            });

            // Team 2
            viewmodel.BingoSetupViewModel2.Initialize.Subscribe(async x =>
            {
                await _gameWindowViewModel.GameViewModel.Team2ViewModel.BingoViewModel.FillInitialBalls();
                viewmodel.BingoSetupViewModel2.State = BingoCardState.Filled;
            });

            viewmodel.BingoSetupViewModel2.ClearBalls.Subscribe(async x =>
            {
                await _gameWindowViewModel.GameViewModel.Team2ViewModel.BingoViewModel.ClearBalls();
                _gameWindowViewModel.GameViewModel.Team2ViewModel.CreateNewBingoCard();
                viewmodel.BingoSetupViewModel2.State = BingoCardState.Empty;
            });

            viewmodel.BingoSetupViewModel2.BallSubmitted.Subscribe(async x =>
            {
                if (await _gameWindowViewModel.GameViewModel.Team2ViewModel.BingoViewModel.FillBall(x))
                {
                    viewmodel.BingoSetupViewModel2.State = BingoCardState.Won;
                }
            });

        }

        private void SubscribeToWordInput(WordInputViewModel viewmodel)
        {
            viewmodel.NewGame.Subscribe(async boardViewModel =>
            {
                _gameWindowViewModel.GameViewModel.CountDownStarted.Subscribe(onNext => viewmodel.StartCountDown());
                _gameWindowViewModel.GameViewModel.BoardViewModel = boardViewModel;
            });

            viewmodel.WhenAnyValue(x => x.WordInputtedByUser)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Subscribe(onNext => _gameWindowViewModel.GameViewModel.SetWord(WordFormatter.Format(onNext)));

            viewmodel.Accept.Subscribe(onNext => _gameWindowViewModel.GameViewModel.AcceptWord());
            viewmodel.Reject.Subscribe(onNext => _gameWindowViewModel.GameViewModel.RejectWord());
            viewmodel.TimeOut.Subscribe(onNext => _gameWindowViewModel.GameViewModel.TimeOut());
            viewmodel.AddRowAndSwitchTeam.Subscribe(onNext => _gameWindowViewModel.GameViewModel.AddRowAndSwitchTeam());
            viewmodel.AddBonusLetter.Subscribe(onNext => _gameWindowViewModel.GameViewModel.AddBonusLetter());
        }
    }
}
