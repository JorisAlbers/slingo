using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Game.Bingo;
using Slingo.Game.Score;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic.Word;

namespace Slingo.Game.Word
{
    public class GameViewModel : ReactiveObject
    {
        private readonly Settings _settings;
        private readonly AudioPlaybackEngine _audioPlaybackEngine;
        private string _activeWord;
        private CachedSound _timeOutSound, _winSound, _rejectSound, _newLetterAppearsSound;
        private SlingoLib.Logic.Word.WordGame _wordGame;
        private Random _random;

        public ScoreboardViewModel ScoreBoardTeam1 { get; }
        public ScoreboardViewModel ScoreBoardTeam2 { get; }
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        [Reactive] public BoardViewModel BoardViewModel { get; private set; }
        [Reactive] public BingoViewModel BingoCardTeam1 { get; private set; }
        [Reactive] public BingoViewModel BingoCardTeam2 { get; private set; }

        [Reactive] public object ActiveTeamName { get; private set; }
        
        public ReactiveCommand<Unit,Unit> CountDownStarted { get; } // TODO move to model class
        

        public GameViewModel(Settings settings, Team team1, Team team2,  AudioPlaybackEngine audioPlaybackEngine)
        {
            _random = new Random();
            _settings = settings;

            _audioPlaybackEngine = audioPlaybackEngine;
            _timeOutSound = new CachedSound(@"Resources\Sounds\WordGame\timeout.wav");
            _winSound = new CachedSound(@"Resources\Sounds\WordGame\win.wav");
            _rejectSound = new CachedSound(@"Resources\Sounds\WordGame\rejected.wav");
            _newLetterAppearsSound = new CachedSound(@"Resources\Sounds\WordGame\first_letter_appears.wav");
            
            ScoreBoardTeam1 = new ScoreboardViewModel(team1.Settings.Name, team1.Score, HorizontalAlignment.Left);
            ScoreBoardTeam2 = new ScoreboardViewModel(team2.Settings.Name, team2.Score, HorizontalAlignment.Right);
            SetActiveTeam(settings.StartingTeamIndex);

           

            CountDownStarted = ReactiveCommand.Create(() => new Unit());
        }

        public async Task StartWordGame(string word)
        {
            int activeTeamIndex = 0;
            _wordGame = new WordGame(new WordPuzzle(word), activeTeamIndex);
            BoardViewModel = new BoardViewModel(_settings.WordSize, _audioPlaybackEngine);
            SelectedViewModel = BoardViewModel;
            _audioPlaybackEngine.PlaySound(_newLetterAppearsSound);
            BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public void SetWord(string word)
        {
            if (word.Length < _settings.WordSize)
            {
                word = word + new string('.', _settings.WordSize - word.Length);
            }

            if (word.Length > _settings.WordSize)
            {
                word = word.Substring(0, _settings.WordSize);
            }


            _activeWord = word;

            BoardViewModel.SetWord(word);
        }

        /// <summary>
        /// Accept the word that was previously set
        /// </summary>
        public async Task<WordGameState> AcceptWord()
        {
            var result = _wordGame.Solve(_activeWord);
            await BoardViewModel.AcceptWord(result);

            if (_wordGame.State == WordGameState.Won)
            {
                _audioPlaybackEngine.PlaySound(_winSound);
                if (_wordGame.ActiveTeamIndex == 0)
                {
                    ScoreBoardTeam1.Score += 50;
                }
                else
                {
                    ScoreBoardTeam2.Score += 50;
                }
                return WordGameState.Won;
            }
            
            if (_wordGame.State == WordGameState.Ongoing)
            {
                await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            }

            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }

            return WordGameState.Ongoing;
        }

        /// <summary>
        /// Reject the word that was previously set
        /// </summary>
        public async Task RejectWord()
        {
            _wordGame.Reject();
            if (_wordGame.State == WordGameState.SwitchTeam)
            {
                _audioPlaybackEngine.PlaySound(_rejectSound);
            }
            else
            {
                await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            }

            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }

        }

        public async Task TimeOut()
        {
            _audioPlaybackEngine.PlaySound(_timeOutSound);
            await RejectWord();
            
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        
        private void SetActiveTeam(int index)
        {
            ActiveTeamName = $"TEAM {index + 1}";
            if (index == 0)
            {
                ScoreBoardTeam1.IsActiveTeam = true;
                ScoreBoardTeam2.IsActiveTeam = false;
            }
            else
            {
                ScoreBoardTeam2.IsActiveTeam = true;
                ScoreBoardTeam1.IsActiveTeam = false;
            }
        }
        
        public async Task AddRowAndSwitchTeam()
        {
            // TODO:// Play switch team sound
            await BoardViewModel.AddAdditionalRow();
            SetActiveTeam(_wordGame.ActiveTeamIndex);
            await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            // TODO add bonus letter sound

            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public async Task AddBonusLetter()
        {
            // TODO: Play bonus letter appears sound
            char letter = _wordGame.AddBonusLetter(out int index);
            await BoardViewModel.AddBonusLetter(letter, index);

            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }

        public async Task InitializeBingoCard(int teamIndex, BingoCardSettings settings)
        {
            BingoViewModel viewmodel = new BingoViewModel(settings, _random);
            SetActiveTeam(teamIndex);
            if (teamIndex == 0)
            {
                BingoCardTeam1 = viewmodel;
            }
            else
            {
                BingoCardTeam2 = viewmodel;
            }

            SelectedViewModel = viewmodel;
        }

        public async Task AddBallsToBingoCard(int teamIndex, BingoCardSettings settings)
        {
            BingoViewModel viewmodel = teamIndex == 0 ? BingoCardTeam1 : BingoCardTeam2;
            SelectedViewModel = viewmodel;
            await viewmodel.FillInitialBalls();
        }

        public async Task SubmitBall(int i)
        {
            await ((BingoViewModel)SelectedViewModel).FillBall(i);
        }
    }
}