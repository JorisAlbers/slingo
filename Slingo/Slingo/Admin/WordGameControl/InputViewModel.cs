using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SlingoLib;
using SlingoLib.Serialization;

namespace Slingo.Admin.WordGameControl
{
    public class InputViewModel : ReactiveObject
    {
        private readonly WordRepository _wordRepository;
        private readonly Settings _settings;
        private readonly Random _random;
        private readonly List<string> _words;

        [Reactive] public string WordInputtedByUser { get; set; }
        [Reactive] public string CandidateWord { get; private set; }
        [Reactive] public string CurrentWord { get; private set; }
        [Reactive] public int TimeLeftBeforeTimeOut { get; set; } 
        [Reactive] public bool AutoTimeOut { get; set; }

        public ReactiveCommand<Unit,Unit> Accept { get; }
        public ReactiveCommand<Unit,Unit> Reject { get; }
        public ReactiveCommand<Unit, Unit> GenerateWord { get; }
        public ReactiveCommand<Unit, string> StartGame { get; }
        public ReactiveCommand<Unit, Unit> TimeOut { get; }
        public ReactiveCommand<Unit, Unit> AddRowAndSwitchTeam { get; }


        public InputViewModel(WordRepository wordRepository, Settings settings)
        {
            _wordRepository = wordRepository;
            _settings = settings;
            _words = _wordRepository.Deserialize(settings.WordSize);
            _random = new Random();
            CandidateWord = GetRandomWord();

            var canAccept = this.WhenAnyValue(
                x => x.WordInputtedByUser, (word) =>
                    !string.IsNullOrEmpty(word) && WordFormatter.Format(word).Length == settings.WordSize);

            Accept = ReactiveCommand.Create(() => new Unit(), canAccept);
            Reject = ReactiveCommand.Create(() => new Unit());
            TimeOut = ReactiveCommand.Create(() => new Unit());
            AddRowAndSwitchTeam = ReactiveCommand.Create(() => new Unit());
            
            GenerateWord = ReactiveCommand.Create(() =>
            {
                CandidateWord = GetRandomWord();
                return new Unit();
            });
            
            StartGame = ReactiveCommand.Create(() =>
            {
                CurrentWord = CandidateWord;
                CandidateWord = GetRandomWord();
                return CurrentWord;
            });
        }
        
        public void StartCountDown()
        {
            InternalStartCountDown();
        }
        
        private async Task InternalStartCountDown()
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(_settings.Timeout);
            do
            {
                TimeLeftBeforeTimeOut = (end - DateTime.Now).Seconds;
                if (TimeLeftBeforeTimeOut == 0)
                {
                    if (AutoTimeOut)
                    {
                        if (await TimeOut.CanExecute.FirstAsync())
                            await TimeOut.Execute();
                    }
                    return;
                }

                await Task.Delay(1000);

            } while (TimeLeftBeforeTimeOut > 0);
        }
        
        private string GetRandomWord()
        {
            return _words[_random.Next(0, _words.Count - 1)];
        }
    }
}
