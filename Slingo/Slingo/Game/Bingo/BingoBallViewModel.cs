using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Game.Bingo
{
    [DebuggerDisplay("{Number}")]
    public class BingoBallViewModel : ReactiveObject
    {
        private BingoBallViewModel _internalBall;
        
        public int Number { get; }
        
        [Reactive] public string Text { get; private set; }
        [Reactive] public int ShowPartlyFilledIndex { get; private set; }
        [Reactive] public bool IsMatchPoint { get; set; } 
        [Reactive] public BallState State { get; private set; }
        [Reactive] public bool ShouldFlash { get; private set; }

        [Reactive] public double X { get; set; }
        [Reactive] public double Y { get; set; }
        [Reactive] public double Width { get; set; }
        [Reactive] public double Height { get; set; }


        public BingoBallViewModel(int number)
        {
            Number = number;
            Text = number.ToString();
            State = BallState.Normal;
        }

        public BingoBallViewModel(BingoBallViewModel ball)
        {
            Number = ball.Number;
            Text = ball.Text;
            State = ball.State;

            _internalBall = ball;

            this.WhenAnyValue(x => x._internalBall.Width).Where(x=> x > 0).Subscribe(x => 
                Width = x);
            this.WhenAnyValue(x => x._internalBall.Height).Where(x => x > 0).Subscribe(x => 
                Height = x);

            ball.WhenAnyValue(x => x.IsMatchPoint, (b) => IsMatchPoint = b);
            ball.WhenAnyValue(x => x.Text, (t) => Text =t);
            ball.WhenAnyValue(x => x.State, (s) => State =s);
            ball.WhenAnyValue(x => x.IsMatchPoint, (m) => IsMatchPoint =m);
          
        }

        public async Task Fill()
        {
            IsMatchPoint = false;
            await Flash();
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

        public async Task SetWinState(string letter)
        {
            State = BallState.Won;
            Text = letter;
            await Flash();
        }

        public async Task Flash()
        {
            ShouldFlash = true;
            await Task.Delay(100);
            ShouldFlash = false;
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