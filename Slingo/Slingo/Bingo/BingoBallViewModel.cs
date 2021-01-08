using System.Diagnostics;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Bingo
{
    [DebuggerDisplay("{Number}")]
    public class BingoBallViewModel : ReactiveObject
    {
        public int Number { get; }
        
        [Reactive] public bool IsFilled { get; private set; }
        
        [Reactive] public int ShowPartlyFilledIndex { get; private set; }
        

        public BingoBallViewModel(int number)
        {
            Number = number;
        }

        public async Task Fill()
        {
            ShowPartlyFilledIndex = 1;
            await Task.Delay(100);
            ShowPartlyFilledIndex = 2;
            await Task.Delay(100);
            ShowPartlyFilledIndex = 3;
            await Task.Delay(100);
            ShowPartlyFilledIndex = 4;
            await Task.Delay(100);
            ShowPartlyFilledIndex = 0;
            IsFilled = true;
            await Task.Delay(100);
        }
    }
}