using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Setup
{
    public class SetupViewModel : ReactiveObject
    {
        public TeamViewModel TeamA {get;}
        public TeamViewModel TeamB {get;}
        
        public ReactiveCommand<Unit,Unit> Start { get; }

        public SetupViewModel()
        {
            TeamA = new TeamViewModel("Team A");
            TeamB = new TeamViewModel("Team B");

            Start = ReactiveCommand.Create(() =>
            {
                ; // TODO
            });
        }
    }
}