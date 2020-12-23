using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using ReactiveUI;

namespace Slingo.WordGame
{
    public class BoardViewModel : ReactiveObject
    {
        private SourceList<WordGameRowViewModel> _wordGameRows = new SourceList<WordGameRowViewModel>();

        public ReadOnlyObservableCollection<WordGameRowViewModel> Rows { get; }

        public BoardViewModel(int wordsize, char firstLetter)
        {
            char[] knownLetters = new char[wordsize];
            knownLetters[0] = firstLetter;
            for (int i = 1; i < wordsize; i++)
            {
                knownLetters[i] = ' ';
            }
            
            // The first row is set, others are empty
            _wordGameRows.Add(new WordGameRowViewModel(wordsize, knownLetters));
            for (int i = 1; i < 5; i++)
            {
                _wordGameRows.Add(CreateEmptyRow(wordsize));
            }

            _wordGameRows.Connect().Bind(out var rows).Subscribe();
            Rows = rows;
        }

        private WordGameRowViewModel CreateEmptyRow(int wordSize)
        {
            return new WordGameRowViewModel(wordSize, Enumerable.Repeat(' ', wordSize).ToArray());
        }
    }
}