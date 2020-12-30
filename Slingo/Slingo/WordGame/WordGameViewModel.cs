using System.Threading.Tasks;
using ReactiveUI;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic;

namespace Slingo.WordGame
{
    public class WordGameViewModel : ReactiveObject
    {
        private readonly Settings _settings;
        private SlingoLib.Logic.WordGame _gameLogic;
        private string _knownLetters;
        private string _activeWord;

        public BoardViewModel BoardViewModel { get; }
        
        public ScoreboardViewModel ScoreBoardTeam1 { get; }
        public ScoreboardViewModel ScoreBoardTeam2 { get; }
        

        public WordGameViewModel(Settings settings, string word, AudioPlaybackEngine audioPlaybackEngine)
        {
            _settings = settings;
            ScoreBoardTeam1 = new ScoreboardViewModel(settings.Team1.Name);
            ScoreBoardTeam2 = new ScoreboardViewModel(settings.Team2.Name);
            
            // todo contain gameLogic
            BoardViewModel = new BoardViewModel(settings.WordSize, audioPlaybackEngine);

            _gameLogic = new SlingoLib.Logic.WordGame(word);
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
            var result = _gameLogic.Solve(_activeWord);
            await BoardViewModel.AcceptWord(result);
            // TODO check if correct
            UpdateKnownLetters(result);
            BoardViewModel.StartNextAttempt(_knownLetters);
        }

        /// <summary>
        /// Reject the word that was previously set
        /// </summary>
        public void RejectWord()
        {
            BoardViewModel.StartNextAttempt(_knownLetters);
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