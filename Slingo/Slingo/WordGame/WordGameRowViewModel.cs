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
        
        public void SetLetter(int index, char letter, LetterState state)
        {
            _letters.ReplaceAt(index,new LetterViewModel(letter, state));
        }

        public void SetInitialLetters(string knownLetters)
        {
            for (int i = 0; i < knownLetters.Length; i++)
            {
                _letters.ReplaceAt(i, new LetterViewModel(knownLetters[i], LetterState.DoesNotExistInWord));
            }
        }
    }
}