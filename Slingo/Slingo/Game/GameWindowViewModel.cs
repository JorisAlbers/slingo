using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Slingo.Admin.Bingo;
using Slingo.Game.Bingo;
using Slingo.Game.Word;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic;
using SlingoLib.Serialization;
using Splat;

namespace Slingo.Game
{
    public class GameWindowViewModel : ReactiveObject
    {
        private readonly AudioPlaybackEngine _audioPlaybackEngine;
        private GameViewModel _gameViewModel;
        private Settings _settings;
        private CachedSound _newLetterAppearsSound;
        private Team _team1,_team2;
        private Random _random;
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }

        public ReactiveCommand<Unit,Unit> CountDownStarted { get; } // TODO move to model class

        [Reactive] public BingoViewModel BingoCardTeam1 { get; private set; }
        [Reactive] public BingoViewModel BingoCardTeam2 { get; private set; }
        
        public GameWindowViewModel(AudioPlaybackEngine audioPlaybackEngine)
        {
            _random = new Random();
            _audioPlaybackEngine = audioPlaybackEngine;
            CountDownStarted = ReactiveCommand.Create(() => new Unit());
            _newLetterAppearsSound = new CachedSound(@"Resources\Sounds\WordGame\first_letter_appears.wav");
        }
        
        public async Task InitializeBingoCard(int teamIndex, BingoCardSettings settings)
        {
            BingoViewModel viewmodel = new BingoViewModel(settings, _random);
            if(teamIndex == 0)
            {
                BingoCardTeam1 = viewmodel;
            }
            else
            {
                BingoCardTeam2 = viewmodel;
            }
            
            SelectedViewModel = viewmodel;

            await viewmodel.FillInitialBalls();
        }
        

        public async void StartGame(Settings settings, string word)
        {
            _settings = settings;
            _team1 = new Team(settings.Team1);
            _team2 = new Team(settings.Team2);

            SelectedViewModel = new GameViewModel(settings, _team1, _team2, word, _audioPlaybackEngine);
            
            _audioPlaybackEngine.PlaySound(_newLetterAppearsSound);
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        
        public async void AcceptWord()
        {
            await _gameViewModel.AcceptWord();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        public async void RejectWord()
        {
            await _gameViewModel.RejectWord();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        public void SetWord(string word)
        {
            _gameViewModel.SetWord(word);
        }

        public async void TimeOut()
        {
            await _gameViewModel.TimeOut();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public async void AddRowAndSwitchTeam()
        {
            await _gameViewModel.AddRowAndSwitchTeam();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public async void AddBonusLetter()
        {
            await _gameViewModel.AddBonusLetter();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public async void SubmitBall(int i)
        {
            await ((BingoViewModel)SelectedViewModel).FillBall(i);
        }
    }
}
