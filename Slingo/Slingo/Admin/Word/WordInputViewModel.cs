using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Game.Word;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic.Word;
using SlingoLib.Serialization;

namespace Slingo.Admin.Word
{
    public class WordInputViewModel : ReactiveObject
    {
        private readonly List<string> _words;
        private Settings _settings;
        private readonly Random _random;
        private readonly WordGameViewModel _wordGameViewModel;
        private CancellationTokenSource _countDownCancellationTokenSource = new CancellationTokenSource();
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
        public ReactiveCommand<Unit, Unit> ClearRow { get; }
        public ReactiveCommand<Unit, Unit> AddBonusLetter { get; }
        public ReactiveCommand<Unit, Unit> ShowWord { get; }
        [Reactive] public WordGameStateInfo StateInfo { get; private set; }


        public WordInputViewModel(List<string> words, Settings settings, Random random,  WordGameViewModel wordGameViewModel)
        {
            _words = words;
            _settings = settings;
            _random = random;
            _wordGameViewModel = wordGameViewModel;

            CandidateWord = GetRandomWord();

            var gameIsOngoing = this.WhenAnyValue(x => x.StateInfo.State, (state) => state == WordGameState.Ongoing);

            var canAccept = this.WhenAnyValue(
                x => x.WordInputtedByUser, x=> x.StateInfo.State, (word,state) =>
                    !string.IsNullOrEmpty(word) && 
                    state == WordGameState.Ongoing && 
                    WordFormatter.Format(word).Length == settings.WordSize);
            
           
            Accept = ReactiveCommand.Create(() => new Unit(), canAccept);
            Reject = ReactiveCommand.Create(() => new Unit(), gameIsOngoing);
            TimeOut = ReactiveCommand.Create(() => new Unit(), gameIsOngoing);
            AddRowAndSwitchTeam = ReactiveCommand.Create(() => new Unit(), this.WhenAnyValue(x=>x.StateInfo.Flags , (switchTeamFlags)=> (switchTeamFlags & SwitchTeamFlags.AddRow) == SwitchTeamFlags.AddRow));
            AddBonusLetter = ReactiveCommand.Create(() => new Unit(), this.WhenAnyValue(x => x.StateInfo.Flags,(switchTeamFlags)=> (switchTeamFlags & SwitchTeamFlags.AddBonusLetter) == SwitchTeamFlags.AddBonusLetter));
            ClearRow = ReactiveCommand.Create(()=> new Unit(),this.WhenAnyValue(x => x.StateInfo.Flags, (flags) => (flags & SwitchTeamFlags.ClearRow) == SwitchTeamFlags.ClearRow));
            
            GenerateWord = ReactiveCommand.Create(() =>
            {
                CandidateWord = GetRandomWord();
                return new Unit();
            });
            
            NewGame = ReactiveCommand.CreateFromTask(async () =>
            {
                var cancel = CancelCountDownAndGetNewToken();
                CurrentWord = CandidateWord;
                CandidateWord = GetRandomWord();
                await wordGameViewModel.StartWordGame(CurrentWord);
                StateInfo = new WordGameStateInfo(WordGameState.Ongoing);
                StartCountDown(cancel.Token);
                return Unit.Default;
            });

            ShowWord = ReactiveCommand.Create(() => new Unit(),
                this.WhenAnyValue(x => x.StateInfo.State, (state) => state == WordGameState.Lost));
            
            this.WhenAnyValue(x => x.WordInputtedByUser)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Subscribe(onNext => wordGameViewModel.SetWord(WordFormatter.Format(onNext)));
            
            this.Accept.Subscribe(async onNext =>
            {
                var cancel = CancelCountDownAndGetNewToken();
                StateInfo = await wordGameViewModel.AcceptWord();
                if (StateInfo.State == WordGameState.SwitchTeam && StateInfo.Flags == SwitchTeamFlags.Normal)
                {
                    StateInfo = new WordGameStateInfo(WordGameState.Ongoing);
                }
                
                if (StateInfo.State == WordGameState.Ongoing)
                {
                    StartCountDown(cancel.Token);
                }
            });
            this.Reject.Subscribe(async onNext =>
            {
                var cancel = CancelCountDownAndGetNewToken();
                StateInfo = await wordGameViewModel.RejectWord();
                if (StateInfo.State == WordGameState.Ongoing)
                {
                    StartCountDown(cancel.Token);
                }
            });
            this.TimeOut.Subscribe(async onNext =>
            {
                var cancel = CancelCountDownAndGetNewToken();
                StateInfo = await wordGameViewModel.TimeOut();
                StartCountDown(cancel.Token);
            });
            this.AddRowAndSwitchTeam.Subscribe(async onNext =>
            {
                var cancel = CancelCountDownAndGetNewToken();
                await wordGameViewModel.AddRow();
                if (StateInfo.State == WordGameState.SwitchTeam)
                {
                    if ((StateInfo.Flags & SwitchTeamFlags.AddBonusLetter) != SwitchTeamFlags.AddBonusLetter)
                    {
                        StateInfo = new WordGameStateInfo(WordGameState.Ongoing);
                    }
                }
                
                StartCountDown(cancel.Token);
            });
            this.AddBonusLetter.Subscribe(async onNext =>
            {
                var cancel = CancelCountDownAndGetNewToken();
                await wordGameViewModel.AddBonusLetter();
                StateInfo = new WordGameStateInfo(WordGameState.Ongoing);
                StartCountDown(cancel.Token);
            });

            this.ClearRow.Subscribe(async x =>
            {
                await wordGameViewModel.ClearRow();
            });

            this.ShowWord.Subscribe(onNext =>
            {
                CancelCountDownAndGetNewToken();
                wordGameViewModel.ShowWord(CurrentWord);
            });
        }

        private CancellationTokenSource CancelCountDownAndGetNewToken()
        {
            lock (_countDownCancellationTokenSource)
            {
                _countDownCancellationTokenSource.Cancel();
                _countDownCancellationTokenSource = new CancellationTokenSource();
                return _countDownCancellationTokenSource;
            }
            
        }

        private string GetRandomWord()
        {
            return _words[_random.Next(0, _words.Count - 1)];
        }

        private async Task StartCountDown(CancellationToken cancellationToken)
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(_settings.Timeout);
            do
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                TimeLeftBeforeTimeOut = (end - DateTime.Now).Seconds;
                if (TimeLeftBeforeTimeOut == 0)
                {
                    if (AutoTimeOut)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }
                        if (await TimeOut.CanExecute.FirstAsync())
                            await TimeOut.Execute();
                    }
                    return;
                }

                await Task.Delay(1000);

            } while (TimeLeftBeforeTimeOut > 0);
        }

        public void Clear()
        {
            _wordGameViewModel.Clear();
        }
    }
}
