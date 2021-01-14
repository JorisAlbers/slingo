using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Admin.Bingo
{
    public class BingoInputViewModel : ReactiveObject
    {
        public ReactiveCommand<Unit, int> BallSubmitted { get; }

        [Reactive] public string NumberString { get; set; }

        public BingoInputViewModel()
        {
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
}
