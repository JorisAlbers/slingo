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
        private WordPuzzle _puzzleLogic;
        private string _knownLetters;
        private string _activeWord;
        private CachedSound _timeOutSound;

        public BoardViewModel BoardViewModel { get; }
        
        public ScoreboardViewModel ScoreBoardTeam1 { get; }
        public ScoreboardViewModel ScoreBoardTeam2 { get; }
        

        public WordGameViewModel(Settings settings, string word, AudioPlaybackEngine audioPlaybackEngine)
        {
            _settings = settings;
            _audioPlaybackEngine = audioPlaybackEngine;
            _puzzleLogic = new WordPuzzle(word);
            _timeOutSound = new CachedSound(@"Resources\Sounds\WordGame\timeout.wav");

            ScoreBoardTeam1 = new ScoreboardViewModel(settings.Team1.Name, HorizontalAlignment.Left);
            ScoreBoardTeam2 = new ScoreboardViewModel(settings.Team2.Name, HorizontalAlignment.Right);
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
            var result = _puzzleLogic.Solve(_activeWord);
            await BoardViewModel.AcceptWord(result);
            // TODO check if correct
            UpdateKnownLetters(result);
            await BoardViewModel.StartNextAttempt(_knownLetters);
        }

        /// <summary>
        /// Reject the word that was previously set
        /// </summary>
        public async Task RejectWord()
        {
            await BoardViewModel.StartNextAttempt(_knownLetters);
        }

        public async Task TimeOut()
        {
            _audioPlaybackEngine.PlaySound(_timeOutSound);
            await BoardViewModel.StartNextAttempt(_knownLetters);
        }

        private void UpdateKnownLetters(WordGameEntry wordGameEntry)
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
    }
}