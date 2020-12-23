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
            _letters.Add(new LetterViewModel('N', LetterState.CorrectLocation));
            _letters.Add(new LetterViewModel('O', LetterState.DoesNotExistInWord));
            _letters.Add(new LetterViewModel('T', LetterState.DoesNotExistInWord));
            _letters.Add(new LetterViewModel('E', LetterState.IncorrectLocation));
            _letters.Add(new LetterViewModel('N', LetterState.CorrectLocation));

            _letters.Connect().Bind(out var items).Subscribe();
            Letters = items;
        }
    }
}