using System;
using System.Collections.Generic;
using System.Text;

namespace SlingoLib
{
    public class Settings
    {
        public Team Team2 { get; }

        public Settings(Team team1, Team team2)
        {
            Team2 = team2;
        }
    }
}
