using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Input
{
    public class InputViewModel : ReactiveObject
    {
        [Reactive] public string Word { get; set; }
        
        [Reactive] public bool WordIsAccepted { get; set; }

        private ReactiveCommand<Unit,Unit> Submit { get; }
    }
}
