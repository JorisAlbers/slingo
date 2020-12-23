using System;
using System.Collections.Generic;
using System.Text;

namespace SlingoLib
{
    public class Settings
    {
        public Team Team1 { get; }
        public Team Team2 { get; }
        public int WordSize { get; set; }

        public Settings(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
        }
    }
}
