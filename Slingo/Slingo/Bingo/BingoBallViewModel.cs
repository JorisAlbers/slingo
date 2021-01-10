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
        
        [Reactive] public string Text { get; private set; }
        [Reactive] public int ShowPartlyFilledIndex { get; private set; }
        [Reactive] public bool IsMatchPoint { get; set; } 
        [Reactive] public BallState State { get; private set; }


        public BingoBallViewModel(int number)
        {
            Number = number;
            Text = number.ToString();
            State = BallState.Normal;
        }

        public async Task Fill()
        {
            IsMatchPoint = false;
            ShowPartlyFilledIndex = 1;
            await Task.Delay(100);
            ShowPartlyFilledIndex = 2;
            await Task.Delay(100);
            ShowPartlyFilledIndex = 3;
            await Task.Delay(100);
            ShowPartlyFilledIndex = 4;
            await Task.Delay(100);
            ShowPartlyFilledIndex = 0;
            State = BallState.Filled;
            await Task.Delay(100);
        }

        public void SetWinState(string letter)
        {
            State = BallState.Won;
            Text = letter;
        }
    }

    public enum BallState
    {
        NotSet,
        Normal,
        Filled,
        Won
    }
}