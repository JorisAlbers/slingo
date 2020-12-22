using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Setup
{
    public class TeamViewModel : ReactiveObject
    {
        public string Name { get; }
        [Reactive] public string Player1 { get; set; }
        [Reactive] public string Player2 { get; set; }

        public TeamViewModel(string name)
        {
            Name = name;
        }
    }
}