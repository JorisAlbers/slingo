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

        public WordGameRowViewModel(int wordSize)
        {
            _wordSize = wordSize;
            for (int i = 0; i < wordSize; i++)
            {
                _letters.Add(new LetterViewModel(' ', LetterState.DoesNotExistInWord));
            }

            _letters.Connect().Bind(out var items).Subscribe();
            Letters = items;
        }
    }
}