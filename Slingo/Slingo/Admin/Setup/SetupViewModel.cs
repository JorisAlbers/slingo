using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SlingoLib;

namespace Slingo.Admin.Setup
{
    public class SetupViewModel : ReactiveObject
    {
        private readonly int[] _excludedBallNumbersEven = new int[] {6,10,12,26,34,36,42,46};
        private readonly int[] _excludedBallNumbersOdd = new int[] {1,3,11,19,27,29,43,47};
       
        [Reactive] public int WordSize { get; set; } = 5;
        [Reactive] public int TimeOut { get; set; } = 8;
        [Reactive] public int Rounds { get; set; } = 3;


        public ReactiveCommand<Unit,Settings> Start { get; }

        public SetupViewModel()
        {
          
            Start = ReactiveCommand.Create(() =>
            {
                return new Settings(1,WordSize,TimeOut,Rounds, _excludedBallNumbersEven, _excludedBallNumbersOdd);
            });
        }
    }
}