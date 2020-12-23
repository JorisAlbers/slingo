using System;
using System.Collections.ObjectModel;
using DynamicData;
using ReactiveUI;
using SlingoLib.Logic;

namespace Slingo.WordGame
{
    public class WordGameRowViewModel : ReactiveObject
    {
        private readonly int _wordSize;
        private readonly SourceList<LetterViewModel> _letters = new SourceList<LetterViewModel>();

        public ReadOnlyObservableCollection<LetterViewModel> Letters { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wordSize"></param>
        /// <param name="knownLetters"> space if not yet known</param>
        public WordGameRowViewModel(int wordSize, char[] knownLetters)
        {
            _wordSize = wordSize;
            foreach (char knownLetter in knownLetters)
            {
                _letters.Add(new LetterViewModel(knownLetter, LetterState.DoesNotExistInWord));
            }
            
            _letters.Connect().Bind(out var items).Subscribe();
            Letters = items;
        }
    }
}