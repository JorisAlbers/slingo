using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Admin.Bingo
{
    public class BingoSetupViewModel : ReactiveObject
    {
        public int TeamIndex { get; }
        public ReactiveCommand<Unit, Unit> Initialize { get;}
        public ReactiveCommand<Unit, Unit> ClearBalls { get;}

        public ReactiveCommand<Unit, int> BallSubmitted { get; }

        [Reactive] public string NumberString { get; set; }


        public BingoSetupViewModel(int teamIndex)
        {
            TeamIndex = teamIndex + 1;

            Initialize = ReactiveCommand.Create(() => Unit.Default);
            ClearBalls = ReactiveCommand.Create(() => Unit.Default);

            var canSubmitBall = this.WhenAnyValue(x => x.NumberString,
                (number) =>
                {
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
}