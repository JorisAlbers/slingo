using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Splat;

namespace Slingo.Game
{
    public class GameWindowViewModel : ReactiveObject
    {
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        
        public GameWindowViewModel()
        {
        }
        
    }
}
