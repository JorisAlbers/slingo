﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using ReactiveUI;

namespace Slingo.WordGame
{
    public class BoardViewModel : ReactiveObject
    {
        private int attemptIndex = 0;
        private SourceList<WordGameRowViewModel> _wordGameRows = new SourceList<WordGameRowViewModel>();

        public ReadOnlyObservableCollection<WordGameRowViewModel> Rows { get; }

        public BoardViewModel(int wordsize)
        {
            for (int i = 0; i < wordsize; i++)
            {
                _wordGameRows.Add(new WordGameRowViewModel(wordsize));
            }
            
            _wordGameRows.Connect().Bind(out var rows).Subscribe();
            Rows = rows;
        }

        public void StartNextAttempt(string knownLetters)
        {
            WordGameRowViewModel viewmodel = _wordGameRows.Items.ElementAt(attemptIndex);
            viewmodel.SetInitialLetters(knownLetters);

        }
    }
}