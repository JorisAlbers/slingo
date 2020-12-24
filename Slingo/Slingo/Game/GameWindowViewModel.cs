using System;
using System.Linq;
using System.Windows.Automation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Slingo.WordGame;
using SlingoLib;
using SlingoLib.Logic;
using SlingoLib.Serialization;
using Splat;

namespace Slingo.Game
{
    public class GameWindowViewModel : ReactiveObject
    {
        private readonly WordRepository _wordRepository;
        private BoardViewModel _boardViewModel;
        private Settings _settings;
        private SlingoLib.Logic.WordGame _gameLogic;
        private string _word;
        private Random _random;
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        
        public GameWindowViewModel(WordRepository wordRepository)
        {
            _wordRepository = wordRepository;
            _random = new Random();
        }

        public void StartGame(Settings settings)
        {
            _settings = settings;
            
            // TODO The next lines should all move to the wordGameViewModel.
            // THis should also keep track of the scores etc, next to the board.
            //WordGameViewModel viewmodel = new WordGameViewModel(settings);
            var words = _wordRepository.Deserialize(5);
            string word = words[_random.Next(0, words.Count - 1)];
            _gameLogic = new SlingoLib.Logic.WordGame(word);
            
            _boardViewModel = new BoardViewModel(settings.WordSize);
            SelectedViewModel = _boardViewModel;
            _boardViewModel.StartNextAttempt($"{word[0]}....");
        }

        public void SetWord(string word)
        {
            _word = word;
            if (word.Length < _settings.WordSize)
            {
                word = word + new string('.', _settings.WordSize - word.Length);
            }

            if (word.Length > _settings.WordSize)
            {
                word = word.Substring(0, _settings.WordSize);
            }

            _boardViewModel.SetWord(word);
        }

        /// <summary>
        /// Accept the word that was previously set
        /// </summary>
        public void AcceptWord()
        {
            var result = _gameLogic.Solve(_word);
            _boardViewModel.AcceptWord(result);
            // TODO check if correct
            _boardViewModel.StartNextAttempt(ConvertWordEntryToKnownLetters(result));
        }

        private string ConvertWordEntryToKnownLetters(WordGameEntry wordGameEntry)
        {
            return new string(wordGameEntry.LetterEntries.Select(x =>
                x.State == LetterState.CorrectLocation ? x.Letter : '.').ToArray());
        }

        /// <summary>
        /// Reject the word that was previously set
        /// </summary>
        public void RejectWord()
        {
            // TODO ignore and move to next
        }
    }
}
