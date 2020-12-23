using System.Reactive;
using ReactiveUI;
using SlingoLib;

namespace Slingo.Admin.Setup
{
    public class SetupViewModel : ReactiveObject
    {
        public TeamViewModel TeamA {get;}
        public TeamViewModel TeamB {get;}
        
        public ReactiveCommand<Unit,Settings> Start { get; }

        public SetupViewModel()
        {
            TeamA = new TeamViewModel("Team A");
            TeamB = new TeamViewModel("Team B");

            Start = ReactiveCommand.Create(() =>
            {
                Team team1 = new Team(TeamA.Name, TeamA.Player1, TeamA.Player2);
                Team team2 = new Team(TeamB.Name, TeamB.Player1, TeamB.Player2);
                return new Settings(team1, team2,5);
            });
        }
    }
}