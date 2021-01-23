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
                
                InputViewModel inputViewModel = new InputViewModel(new WordRepository(new FileSystem(), @"Resources\basiswoorden-gekeurd.txt"), settings);
                // Word input
                inputViewModel.WhenAnyValue(x => x.WordInputViewModel).Where(x=>x!=null).Subscribe(SubscribeToWordInput);

                // Bingo input
                SubscribeToBingoInput(inputViewModel.BingoAdminPanelViewModel);
                
                SelectedViewModel = inputViewModel;
            });

            SelectedViewModel = _setupViewModel;

            _gameWindowViewModel = new GameWindowViewModel();
            var view = Locator.Current.GetService<IViewFor<GameWindowViewModel>>();
            var window = view as ReactiveWindow<GameWindowViewModel>;
            window.ViewModel = _gameWindowViewModel;
            window.Show();
        }

        private void SubscribeToBingoInput(BingoAdminPanelViewModel viewmodel)
        {
            viewmodel.SetupViewModelTeam1.ShowPanel.Subscribe(x =>
                _gameWindowViewModel.GameViewModel.InitializeBingoCard(0, x));
            
            viewmodel.SetupViewModelTeam1.Initialize.Subscribe(x =>
                  _gameWindowViewModel.GameViewModel.AddBallsToBingoCard(0, x));

            viewmodel.SetupViewModelTeam1.ClearBalls.Subscribe(x =>
                _gameWindowViewModel.GameViewModel.ClearBallsOfBingoCard(0));

            viewmodel.SetupViewModelTeam2.ShowPanel.Subscribe(x =>
                _gameWindowViewModel.GameViewModel.InitializeBingoCard(1, x));

            viewmodel.SetupViewModelTeam2.Initialize.Subscribe(x =>
                _gameWindowViewModel.GameViewModel.AddBallsToBingoCard(1, x));

            viewmodel.SetupViewModelTeam2.ClearBalls.Subscribe(x =>
                _gameWindowViewModel.GameViewModel.ClearBallsOfBingoCard(1));


            viewmodel.BingoInputViewModel.BallSubmitted.Subscribe(x => _gameWindowViewModel.GameViewModel.SubmitBall(x));
        }

        private void SubscribeToWordInput(WordInputViewModel viewmodel)
        {
            viewmodel.StartGame.Subscribe(async word =>
            {
                _gameWindowViewModel.GameViewModel.CountDownStarted.Subscribe(onNext =>
                    viewmodel.StartCountDown());
                await _gameWindowViewModel.GameViewModel.StartWordGame(word);
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
