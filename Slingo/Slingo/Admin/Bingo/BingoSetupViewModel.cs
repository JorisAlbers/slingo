using System;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Word;

namespace Slingo.Admin.Bingo
{
    public class BingoSetupViewModel : ReactiveObject
    {
        private readonly TeamState _teamState;
        public ReactiveCommand<Unit, Unit> Initialize { get;}
        public ReactiveCommand<Unit, Unit> ClearBalls { get;}

        public ReactiveCommand<Unit, int> BallSubmitted { get; }
        
        public ReactiveCommand<Unit,bool> GreenBallSubmitted { get; }
        public ReactiveCommand<Unit,Unit> RedBallSubmitted { get; }

        [Reactive] public string NumberString { get; set; }

        [Reactive] public BingoCardState State { get; set; }
        [Reactive] public bool InverseCommand { get; set; }


        public BingoSetupViewModel(TeamState teamState)
        {
            _teamState = teamState;
            Initialize = ReactiveCommand.Create(() => Unit.Default, this.WhenAnyValue(x=> x.State, (state) => state == BingoCardState.Empty));
            ClearBalls = ReactiveCommand.Create(() => Unit.Default, this.WhenAnyValue(x=> x.State, (state) => state == BingoCardState.Won));

            var canSubmitBall = this.WhenAnyValue(x => x.NumberString, x=> x.State, x=>x._teamState,
                (number, state, teamState) =>
                {
                    if (state != BingoCardState.Filled)
                    {
                        return false;
                    }

                    if (!teamState.IsActiveTeam)
                    {
                        return false;
                    }
                    
                    if (int.TryParse(number, out int i))
                    {
                        if (i > 0 && i < 51)
                        {
                            return true;
                        }
                    }
                    return false;
                });

            BallSubmitted = ReactiveCommand.Create(() => int.Parse(NumberString), canSubmitBall);
            GreenBallSubmitted = ReactiveCommand.Create(() => !InverseCommand);
            RedBallSubmitted = ReactiveCommand.Create(()=> Unit.Default);
        }
    }

    public class BingoCardSettings
    {
        public bool EvenNumber { get; }
        public int[] FilledBalls { get; }

        public BingoCardSettings(bool evenNumber, int[] filledBalls)
        {
            EvenNumber = evenNumber;
            FilledBalls = filledBalls;
        }
    }

    public enum BingoCardState
    {
        Empty,
        Filled,
        Won
    }
}