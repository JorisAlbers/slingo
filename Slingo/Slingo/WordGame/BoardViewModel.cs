﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using SlingoLib.Logic;

namespace Slingo.WordGame
{
    public class BoardViewModel : ReactiveObject
    {
        private int attemptIndex = -1;
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

        public async Task StartNextAttempt(string knownLetters)
        {
            WordGameRowViewModel viewmodel = _wordGameRows.Items.ElementAt(++attemptIndex);
            await viewmodel.SetInitialLetters(knownLetters);
        }

        /// <summary>
        /// Sets the word without checking for correctness yet
        /// </summary>
        /// <param name="word"></param>
        public void SetWord(string word)
        {
            WordGameRowViewModel viewmodel = _wordGameRows.Items.ElementAt(attemptIndex);
            for (int i = 0; i < word.Length; i++)
            {
                viewmodel.SetLetter(i,word[i],LetterState.DoesNotExistInWord);
            }
        }

        public async Task AcceptWord(WordGameEntry result)
        {
            WordGameRowViewModel viewmodel = _wordGameRows.Items.ElementAt(attemptIndex);
            for (int i = 0; i < result.LetterEntries.Length; i++)
            {
                viewmodel.SetLetter(i, result.LetterEntries[i].Letter, result.LetterEntries[i].State);

                await Task.Run(() =>
                {
                    switch (result.LetterEntries[i].State)
                    {
                        case LetterState.DoesNotExistInWord:
                            Console.Beep(440,350);
                            break;
                        case LetterState.CorrectLocation:
                            Console.Beep(494,350);
                            break;
                        case LetterState.IncorrectLocation:
                            Console.Beep(262,350);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                });
            }
        }
    }
}