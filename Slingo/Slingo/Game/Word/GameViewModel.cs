using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
        private CachedSound _timeOutSound, _winSound, _rejectSound;
        private SlingoLib.Logic.Word.WordGame _wordGame;

        public BoardViewModel BoardViewModel { get; }
        
        public ScoreboardViewModel ScoreBoardTeam1 { get; }
        public ScoreboardViewModel ScoreBoardTeam2 { get; }
        [Reactive] public object ActiveTeamName { get; private set; }


        public GameViewModel(Settings settings, Team team1, Team team2, string word, AudioPlaybackEngine audioPlaybackEngine)
        {
            _settings = settings;
            _wordGame = new SlingoLib.Logic.Word.WordGame(new WordPuzzle(word), settings.StartingTeamIndex);

            _audioPlaybackEngine = audioPlaybackEngine;
            _timeOutSound = new CachedSound(@"Resources\Sounds\WordGame\timeout.wav");
            _winSound = new CachedSound(@"Resources\Sounds\WordGame\win.wav");
            _rejectSound = new CachedSound(@"Resources\Sounds\WordGame\rejected.wav");

            ScoreBoardTeam1 = new ScoreboardViewModel(team1.Settings.Name, team1.Score, HorizontalAlignment.Left);
            ScoreBoardTeam2 = new ScoreboardViewModel(team2.Settings.Name, team2.Score, HorizontalAlignment.Right);
            SetActiveTeam(settings.StartingTeamIndex);

            BoardViewModel = new BoardViewModel(settings.WordSize, audioPlaybackEngine);
            BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
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

        }

        public async Task TimeOut()
        {
            _audioPlaybackEngine.PlaySound(_timeOutSound);
            await RejectWord();
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
        }

        public async Task AddBonusLetter()
        {
            // TODO: Play bonus letter appears sound
            char letter = _wordGame.AddBonusLetter(out int index);
            await BoardViewModel.AddBonusLetter(letter, index);
        }
    }
}