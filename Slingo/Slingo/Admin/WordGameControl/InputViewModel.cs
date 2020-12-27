using System;
using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SlingoLib;
using SlingoLib.Serialization;

namespace Slingo.Admin.WordGameControl
{
    public class InputViewModel : ReactiveObject
    {
        private readonly WordRepository _wordRepository;
        private readonly Random _random;
        private readonly List<string> _words;

        [Reactive] public string Word { get; set; }
        
        public ReactiveCommand<Unit,Unit> Accept { get; }
        public ReactiveCommand<Unit,Unit> Reject { get; }

        [Reactive] public string NextWord { get; private set; }

        public ReactiveCommand<Unit, Unit> GenerateWord;
        public ReactiveCommand<Unit, string> StartGame;

        public InputViewModel(WordRepository wordRepository, int wordSize)
        {
            _wordRepository = wordRepository;
            _words = _wordRepository.Deserialize(wordSize);
            _random = new Random();
            NextWord = GetRandomWord();


            var canAccept = this.WhenAnyValue(
                x => x.Word, (word) =>
                    !string.IsNullOrEmpty(word) && WordFormatter.Format(word).Length == wordSize);

            Accept = ReactiveCommand.Create(() => new Unit(), canAccept);
            Reject = ReactiveCommand.Create(() => new Unit());
            GenerateWord = ReactiveCommand.Create(() =>
            {
                NextWord = GetRandomWord();
                return new Unit();
            });
            StartGame = ReactiveCommand.Create(() => NextWord);
        }
        
        

        private string GetRandomWord()
        {
            return _words[_random.Next(0, _words.Count - 1)];
        }
    }
}
