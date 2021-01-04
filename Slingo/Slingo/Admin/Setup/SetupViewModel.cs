using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SlingoLib;

namespace Slingo.Admin.Setup
{
    public class SetupViewModel : ReactiveObject
    {
        public TeamViewModel TeamA {get;}
        public TeamViewModel TeamB {get;}

        [Reactive] public int WordSize { get; set; } = 5;
        [Reactive] public int TimeOut { get; set; } = 8;
        [Reactive] public int Rounds { get; set; } = 3;


        public ReactiveCommand<Unit,Settings> Start { get; }

        public SetupViewModel()
        {
            TeamA = new TeamViewModel("Team A");
            TeamB = new TeamViewModel("Team B");

            Start = ReactiveCommand.Create(() =>
            {
                TeamSettings team1 = new TeamSettings(TeamA.Name, TeamA.Player1, TeamA.Player2);
                TeamSettings team2 = new TeamSettings(TeamB.Name, TeamB.Player1, TeamB.Player2);
                return new Settings(team1, team2,1,WordSize,TimeOut,Rounds);
            });
        }
    }
}