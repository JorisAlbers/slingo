using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
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
        private WordGameViewModel _wordGameViewModel;
        private Settings _settings;
        private CachedSound _newLetterAppearsSound;
        private Team _team1,_team2;
        private BingoViewModel _bingoViewModel;
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }

        public ReactiveCommand<Unit,Unit> CountDownStarted { get; } // TODO move to model class

        public GameWindowViewModel(AudioPlaybackEngine audioPlaybackEngine)
        {
            _audioPlaybackEngine = audioPlaybackEngine;
            CountDownStarted = ReactiveCommand.Create(() => new Unit());
            _newLetterAppearsSound = new CachedSound(@"Resources\Sounds\WordGame\first_letter_appears.wav");
        }

        public async void StartGame(Settings settings, string word)
        {
            _settings = settings;
            _team1 = new Team(settings.Team1);
            _team2 = new Team(settings.Team2);

            //BingoViewModel bingoViewModel = new BingoViewModel(true, settings.ExcludedBallNumbersEven, new Random());
            _bingoViewModel = new BingoViewModel(false, settings.ExcludedBallNumbersOdd, new Random());
            SelectedViewModel = _bingoViewModel;
            _bingoViewModel.FillInitialBalls();
            
            //SelectedViewModel = new BingoViewModel(false, new Random(), new int[]{1,3,5,7,9});

            //_wordGameViewModel = new WordGameViewModel(settings, _team1, _team2, word, _audioPlaybackEngine);
            //SelectedViewModel = _wordGameViewModel;
            _audioPlaybackEngine.PlaySound(_newLetterAppearsSound);
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        
        public async void AcceptWord()
        {
            await _wordGameViewModel.AcceptWord();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        public async void RejectWord()
        {
            await _wordGameViewModel.RejectWord();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        public void SetWord(string word)
        {
            _wordGameViewModel.SetWord(word);
        }

        public async void TimeOut()
        {
            await _wordGameViewModel.TimeOut();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public async void AddRowAndSwitchTeam()
        {
            await _wordGameViewModel.AddRowAndSwitchTeam();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public async void AddBonusLetter()
        {
            await _wordGameViewModel.AddBonusLetter();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public async void SubmitBall(int i)
        {
            await _bingoViewModel.FillBall(i);
        }
    }
}
