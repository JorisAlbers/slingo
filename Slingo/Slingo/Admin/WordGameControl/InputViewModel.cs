using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Admin.WordGameControl
{
    public class InputViewModel : ReactiveObject
    {
        [Reactive] public string Word { get; set; }

        public ReactiveCommand<Unit,Unit> Accept { get; }
        public ReactiveCommand<Unit,Unit> Reject { get; }

        public InputViewModel()
        {
            Accept = ReactiveCommand.Create(() => new Unit());
            Reject = ReactiveCommand.Create(() => new Unit());
        }
    }
}
