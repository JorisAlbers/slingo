using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Convert = System.Convert;

namespace Slingo.Admin.Word
{
    public class GameState : ReactiveObject
    {
        public TeamState Team1 { get;  }
        public TeamState Team2 { get;  }
        public GameSection CurrentSection { get; set; }

        [Reactive] public int ActiveTeam { get; set; }

        public GameState(TeamState team1, TeamState team2)
        {
            Team1 = team1;
            Team2 = team2;

            this.WhenAnyValue(x => x.Team1.IsActiveTeam, x=>x.Team2.IsActiveTeam).Subscribe(x =>
            {
                ActiveTeam = x.Item1 ? 1 : 2;
            });
        }
        public void SwitchActiveTeam()
        {
            Team1.IsActiveTeam = !Team1.IsActiveTeam;
            Team2.IsActiveTeam = !Team2.IsActiveTeam;
        }
    }

    public class TeamState : ReactiveObject
    {
        [Reactive] public int Score { get; set; }
        
        [Reactive] public bool IsActiveTeam { get; set; }

        public TeamState(bool isActiveTeam)
        {
            IsActiveTeam = isActiveTeam;
        }
    }
}