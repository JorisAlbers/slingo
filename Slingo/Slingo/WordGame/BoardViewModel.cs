﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using Slingo.Sound;
using SlingoLib.Logic;
using SlingoLib.Logic.Word;

namespace Slingo.WordGame
{
    public class BoardViewModel : ReactiveObject
    {
        private readonly AudioPlaybackEngine _audioPlaybackEngine;
        private int attemptIndex = -1;
        private readonly SourceList<WordGameRowViewModel> _wordGameRows = new SourceList<WordGameRowViewModel>();
        private readonly CachedSound _incorrectSound;
        private readonly CachedSound _incorrectLocationSound;
        private readonly CachedSound _correctSound;

        public ReadOnlyObservableCollection<WordGameRowViewModel> Rows { get; }

        public BoardViewModel(int wordsize, AudioPlaybackEngine audioPlaybackEngine)
        {
            _audioPlaybackEngine = audioPlaybackEngine;
            _correctSound = new CachedSound(@"Resources\Sounds\WordGame\correct.wav");
            _incorrectSound = new CachedSound(@"Resources\Sounds\WordGame\incorrect.wav");
            _incorrectLocationSound = new CachedSound(@"Resources\Sounds\WordGame\incorrect_location.wav");
           
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
            CombinedSoundSampleProvider soundProvider = SetupWordGameEntrySounds(result, 200);

            WordGameRowViewModel viewmodel = _wordGameRows.Items.ElementAt(attemptIndex);
            _audioPlaybackEngine.PlaySound(soundProvider);
            for (int i = 0; i < result.LetterEntries.Length; i++)
            {
                viewmodel.SetLetter(i, result.LetterEntries[i].Letter, result.LetterEntries[i].State);
                await Task.Delay(200);
            }
        }
        
        private CombinedSoundSampleProvider SetupWordGameEntrySounds(WordGameEntry entry, int millisecondsPerSound)
        {
            CachedSound[] sounds = new CachedSound[entry.LetterEntries.Length];
            for (int i = 0; i < entry.LetterEntries.Length; i++)
            {
                switch (entry.LetterEntries[i].State)
                {
                    case LetterState.DoesNotExistInWord:
                        sounds[i] = _incorrectSound;
                        break;
                    case LetterState.CorrectLocation:
                        sounds[i] = _correctSound;
                        break;
                    case LetterState.IncorrectLocation:
                        sounds[i] = _incorrectLocationSound;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new CombinedSoundSampleProvider(sounds, millisecondsPerSound);
        }
    }
}