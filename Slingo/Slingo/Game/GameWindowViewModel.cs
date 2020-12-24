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
        private string _knownLetters;
        private string _activeWord;
        private SlingoLib.Logic.WordGame _gameLogic;
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        

        public void StartGame(Settings settings, string word)
        {
            _settings = settings;
            // TODO The next lines should all move to the wordGameViewModel.
            // THis should also keep track of the scores etc, next to the board.
            //WordGameViewModel viewmodel = new WordGameViewModel(settings);
            _gameLogic = new SlingoLib.Logic.WordGame(word);
            _boardViewModel = new BoardViewModel(settings.WordSize);
            SelectedViewModel = _boardViewModel;
            _knownLetters = word[0] + new string('.', word.Length - 1);
            _boardViewModel.StartNextAttempt(_knownLetters);
        }

        public void SetWord(string word)
        {
            _activeWord = word;
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
            var result = _gameLogic.Solve(_activeWord);
            _boardViewModel.AcceptWord(result);
            // TODO check if correct
            UpdateKnownLetters(result);
            _boardViewModel.StartNextAttempt(_knownLetters);
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

        /// <summary>
        /// Reject the word that was previously set
        /// </summary>
        public void RejectWord()
        {
            // TODO ignore and move to next
        }
    }
}
