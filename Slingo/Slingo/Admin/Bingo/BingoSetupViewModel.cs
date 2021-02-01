using System.Reactive;
using System.Windows.Input;
using ReactiveUI;

namespace Slingo.Admin.Bingo
{
    public class BingoSetupViewModel : ReactiveObject
    {
        public int TeamIndex { get; }
        public ReactiveCommand<Unit, Unit> Initialize { get;}
        public ReactiveCommand<Unit, Unit> ClearBalls { get;}

        public BingoSetupViewModel(int teamIndex)
        {
            TeamIndex = teamIndex + 1;

            Initialize = ReactiveCommand.Create(() => Unit.Default);
            ClearBalls = ReactiveCommand.Create(() => Unit.Default);
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