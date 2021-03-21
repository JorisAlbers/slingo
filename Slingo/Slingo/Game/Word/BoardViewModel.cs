using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using Slingo.Sound;
using SlingoLib.Logic.Word;

namespace Slingo.Game.Word
{
    public class BoardViewModel : ReactiveObject
    {
        private readonly AudioPlaybackEngine _audioPlaybackEngine;
        private int attemptIndex = -1;
        private readonly SourceList<WordGameRowViewModel> _wordGameRows = new SourceList<WordGameRowViewModel>();
        private readonly CachedSound _incorrectSound;
        private readonly CachedSound _incorrectLocationSound;
        private readonly CachedSound _correctSound;
        private CachedSound _newRowAppears;

        public ReadOnlyObservableCollection<WordGameRowViewModel> Rows { get; }

        public BoardViewModel(int wordsize, AudioPlaybackEngine audioPlaybackEngine)
        {
            _audioPlaybackEngine = audioPlaybackEngine;
            _correctSound = new CachedSound(@"Resources\Sounds\WordGame\correct.wav");
            _incorrectSound = new CachedSound(@"Resources\Sounds\WordGame\incorrect.wav");
            _incorrectLocationSound = new CachedSound(@"Resources\Sounds\WordGame\incorrect_location.wav");
            _newRowAppears = new CachedSound(@"Resources\Sounds\WordGame\additional_row_appears.wav");
           
            for (int i = 0; i < wordsize; i++)
            {
                _wordGameRows.Add(new WordGameRowViewModel(wordsize));
            }
            
            _wordGameRows.Connect().Bind(out var rows).Subscribe();
            Rows = rows;
        }

        public async Task StartNextAttempt(string knownLetters)
        {
            attemptIndex = Math.Min(attemptIndex + 1, 4);
            WordGameRowViewModel viewmodel = _wordGameRows.Items.ElementAt(attemptIndex);
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

        public async Task AcceptWord(WordPuzzleEntry result)
        {
            CombinedSoundSampleProvider soundProvider = SetupWordGameEntrySounds(result.LetterEntries.Select(x=>x.State), 200);

            WordGameRowViewModel viewmodel = _wordGameRows.Items.ElementAt(attemptIndex);
            _audioPlaybackEngine.PlaySound(soundProvider);
            for (int i = 0; i < result.LetterEntries.Length; i++)
            {
                viewmodel.SetLetter(i, result.LetterEntries[i].Letter, result.LetterEntries[i].State);
                await Task.Delay(200);
            }
        }

        public async Task ShowWord(string word)
        {
            CombinedSoundSampleProvider soundProvider = SetupWordGameEntrySounds(Enumerable.Repeat(LetterState.CorrectLocation,word.Length), 200);

            WordGameRowViewModel viewmodel = _wordGameRows.Items.ElementAt(attemptIndex);
            _audioPlaybackEngine.PlaySound(soundProvider);
            for (int i = 0; i < word.Length; i++)
            {
                viewmodel.SetLetter(i, word[i], LetterState.DoesNotExistInWord);
                await Task.Delay(200);
            }
        }

        public async Task AddAdditionalRow()
        {
            _audioPlaybackEngine.PlaySound(_newRowAppears);
            _wordGameRows.RemoveAt(0);
            _wordGameRows.Add(new WordGameRowViewModel(_wordGameRows.Items.First().Letters.Count));
            await Task.Delay(200);
        }

        public async Task AddBonusLetter(char letter, int index)
        {
            // The knownLetters now has a bonus letter added
            var viewmodel = _wordGameRows.Items.ElementAt(attemptIndex);
            viewmodel.SetLetter(index,letter, LetterState.DoesNotExistInWord);
            await Task.Delay(200);
        }

        private CombinedSoundSampleProvider SetupWordGameEntrySounds(IEnumerable<LetterState> entry,
            int millisecondsPerSound)
        {
            var sounds = entry.Select(x =>
            {
                switch (x)
                {
                    case LetterState.DoesNotExistInWord:
                        return _incorrectSound;
                    case LetterState.CorrectLocation:
                        return _correctSound;
                    case LetterState.IncorrectLocation:
                        return _incorrectLocationSound;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }).ToArray();

            return new CombinedSoundSampleProvider(sounds, millisecondsPerSound);
        }

        public async Task ClearRow(string knownLetters)
        {
            var viewmodel = new WordGameRowViewModel(_wordGameRows.Items.First().Letters.Count);
            _wordGameRows.ReplaceAt(attemptIndex,viewmodel);
            await viewmodel.SetInitialLetters(knownLetters);
        }
    }
}