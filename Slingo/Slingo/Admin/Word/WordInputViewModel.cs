using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Game.Word;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Serialization;

namespace Slingo.Admin.Word
{
    public class WordInputViewModel : ReactiveObject
    {
        private readonly List<string> _words;
        private Settings _settings;
        private readonly Random _random;
        [Reactive] public string WordInputtedByUser { get; set; }
        [Reactive] public string CandidateWord { get; private set; }
        [Reactive] public string CurrentWord { get; private set; }
        [Reactive] public int TimeLeftBeforeTimeOut { get; set; }
        [Reactive] public bool AutoTimeOut { get; set; }

        public ReactiveCommand<Unit, Unit> Accept { get; }
        public ReactiveCommand<Unit, Unit> Reject { get; }
        public ReactiveCommand<Unit, Unit> GenerateWord { get; }
        public ReactiveCommand<Unit, Unit> NewGame { get; }
        public ReactiveCommand<Unit, Unit> TimeOut { get; }
        public ReactiveCommand<Unit, Unit> AddRowAndSwitchTeam { get; }
        public ReactiveCommand<Unit, Unit> AddBonusLetter { get; }

        public WordInputViewModel(List<string> words, Settings settings, Random random,  WordGameViewModel wordGameViewModel)
        {
            _words = words;
            _settings = settings;
            _random = random;

            CandidateWord = GetRandomWord();

            var canAccept = this.WhenAnyValue(
                x => x.WordInputtedByUser, (word) =>
                    !string.IsNullOrEmpty(word) && WordFormatter.Format(word).Length == settings.WordSize);
           
            Accept = ReactiveCommand.Create(() => new Unit(), canAccept);
            Reject = ReactiveCommand.Create(() => new Unit());
            TimeOut = ReactiveCommand.Create(() => new Unit());
            AddRowAndSwitchTeam = ReactiveCommand.Create(() => new Unit());
            AddBonusLetter = ReactiveCommand.Create(() => new Unit());

            GenerateWord = ReactiveCommand.Create(() =>
            {
                CandidateWord = GetRandomWord();
                return new Unit();
            });
            
            NewGame = ReactiveCommand.CreateFromTask(async () =>
            {
                CurrentWord = CandidateWord;
                CandidateWord = GetRandomWord();
                await wordGameViewModel.StartWordGame(CurrentWord);
                StartCountDown();
                return Unit.Default;
            });
            
            this.WhenAnyValue(x => x.WordInputtedByUser)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Subscribe(onNext => wordGameViewModel.SetWord(WordFormatter.Format(onNext)));
            
            this.Accept.Subscribe(async onNext =>
            {
                await wordGameViewModel.AcceptWord();
                StartCountDown();

            });
            this.Reject.Subscribe(async onNext =>
            {
                await wordGameViewModel.RejectWord();
                StartCountDown();
            });
            this.TimeOut.Subscribe(async onNext =>
            {
                await wordGameViewModel.TimeOut();
                StartCountDown();
            });
            this.AddRowAndSwitchTeam.Subscribe(async onNext =>
            {
                await wordGameViewModel.AddRowAndSwitchTeam();
                StartCountDown();
            });
            this.AddBonusLetter.Subscribe(async onNext =>
            {
                await wordGameViewModel.AddBonusLetter();
                StartCountDown();
            });
        }
        private string GetRandomWord()
        {
            return _words[_random.Next(0, _words.Count - 1)];
        }

        private async Task StartCountDown()
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
    }
}
