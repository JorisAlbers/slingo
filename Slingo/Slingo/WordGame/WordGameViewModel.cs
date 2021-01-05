using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic;
using SlingoLib.Logic.Word;

namespace Slingo.WordGame
{
    public class WordGameViewModel : ReactiveObject
    {
        private readonly Settings _settings;
        private readonly AudioPlaybackEngine _audioPlaybackEngine;
        private string _knownLetters;
        private string _activeWord;
        private CachedSound _timeOutSound, _winSound, _rejectSound;
        private SlingoLib.Logic.Word.WordGame _wordGame;

        public BoardViewModel BoardViewModel { get; }
        
        public ScoreboardViewModel ScoreBoardTeam1 { get; }
        public ScoreboardViewModel ScoreBoardTeam2 { get; }
        

        public WordGameViewModel(Settings settings, string word, AudioPlaybackEngine audioPlaybackEngine)
        {
            _settings = settings;
            _wordGame = new SlingoLib.Logic.Word.WordGame(new WordPuzzle(word), settings.StartingTeamIndex);

            _audioPlaybackEngine = audioPlaybackEngine;
            _timeOutSound = new CachedSound(@"Resources\Sounds\WordGame\timeout.wav");
            _winSound = new CachedSound(@"Resources\Sounds\WordGame\win.wav");
            _rejectSound = new CachedSound(@"Resources\Sounds\WordGame\rejected.wav");

            ScoreBoardTeam1 = new ScoreboardViewModel(settings.Team1.Name, HorizontalAlignment.Left);
            ScoreBoardTeam2 = new ScoreboardViewModel(settings.Team2.Name, HorizontalAlignment.Right);
            SetActiveTeam(settings.StartingTeamIndex);

            BoardViewModel = new BoardViewModel(settings.WordSize, audioPlaybackEngine);
            _knownLetters = word[0] + new string('.', word.Length - 1);
            BoardViewModel.StartNextAttempt(_knownLetters);
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
        public async Task AcceptWord()
        {
            var result = _wordGame.Solve(_activeWord);
            await BoardViewModel.AcceptWord(result);

            if (_wordGame.State == WordGameState.Won)
            {
                // TODO:
                _audioPlaybackEngine.PlaySound(_winSound);
                // add score
                // End game
                return;
            }
            
            UpdateKnownLetters(result);
            if (_wordGame.State == WordGameState.Ongoing)
            {
                await BoardViewModel.StartNextAttempt(_knownLetters);
            }
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
                await BoardViewModel.StartNextAttempt(_knownLetters);
            }

        }

        public async Task TimeOut()
        {
            _audioPlaybackEngine.PlaySound(_timeOutSound);
            await RejectWord();
        }

        private void UpdateKnownLetters(WordPuzzleEntry wordGameEntry)
        {
            char[] knownLetters = _knownLetters.ToCharArray();
            for (int i = 0; i < knownLetters.Length; i++)
            {
                if (knownLetters[i] == '.')
                {
                    if (wordGameEntry.LetterEntries[i].State == LetterState.CorrectLocation)
                    {
                        knownLetters[i] = wordGameEntry.LetterEntries[i].Letter;
                    }
                }
            }

            _knownLetters = new string(knownLetters);
        }

        private void SetActiveTeam(int index)
        {
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
            await BoardViewModel.StartNextAttempt(_knownLetters);
            // TODO add bonus letter sound
        }
    }
}