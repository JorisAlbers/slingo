using System.Diagnostics;
using ReactiveUI;

namespace Slingo.Bingo
{
    [DebuggerDisplay("{Number}")]
    public class BingoBallViewModel : ReactiveObject
    {
        public int Number { get; }
        
        public bool IsFilled { get; set; }

        public BingoBallViewModel(int number)
        {
            Number = number;
        }
    }
}