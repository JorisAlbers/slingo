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

            this.WhenAnyValue(x => x._internalBall.IsMatchPoint).Subscribe(x=>IsMatchPoint = x);
            this.WhenAnyValue(x => x._internalBall.Text).Subscribe(x=>Text = x);
            this.WhenAnyValue(x => x._internalBall.State).Subscribe(x=> State =x);
        }

        public async Task Fill()
        {
            IsMatchPoint = false;
            await Flash();
            await Down();
            ShowPartlyFilledIndex = 0;
            State = BallState.Filled;
            await Task.Delay(100);
        }

        public async Task FillLong()
        {
            IsMatchPoint = false;
            await Flash();

            // down
            await Down();
            await Up();
            await Down();
            await Up();
            await Down();
            // end
            ShowPartlyFilledIndex = 0;
            State = BallState.Filled;
            await Task.Delay(100);
        }

        private async Task Down()
        {
            ShowPartlyFilledIndex = 1;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 2;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 3;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 4;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 5;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 6;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 7;
            await Task.Delay(20);
        }

        private async Task Up()
        {
            ShowPartlyFilledIndex = 6;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 5;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 4;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 3;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 2;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 1;
            await Task.Delay(10);
            ShowPartlyFilledIndex = 0;
            await Task.Delay(20);
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