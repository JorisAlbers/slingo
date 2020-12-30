using System;
using System.Collections.Generic;
using System.Text;
using SlingoLib.Logic;

namespace SlingoLib
{
    public class Settings
    {
        public Team Team1 { get; }
        public Team Team2 { get; }
        public int WordSize { get; }
        
        /// <summary> Seconds available to come up with the next word entry. </summary>
        public int Timeout { get; }

        public Settings(Team team1, Team team2, int wordSize, int timeout)
        {
            Team1 = team1;
            Team2 = team2;
            WordSize = wordSize;
            Timeout = timeout;
        }
    }
}
