using System;
using System.Collections.ObjectModel;
using DynamicData;
using ReactiveUI;

namespace Slingo.WordGame
{
    public class BoardViewModel : ReactiveObject
    {
        private SourceList<WordGameRowViewModel> _wordGameRows = new SourceList<WordGameRowViewModel>();

        public ReadOnlyObservableCollection<WordGameRowViewModel> Rows { get; }

        public BoardViewModel(int wordsize)
        {
            // create 5 empty rows
            for (int i = 0; i < 5; i++)
            {
                _wordGameRows.Add(new WordGameRowViewModel(wordsize));
            }

            _wordGameRows.Connect().Bind(out var rows).Subscribe();
            Rows = rows;
        }
    }
}