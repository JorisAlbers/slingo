using System;
using System.Collections.Generic;
using System.Text;
using SlingoLib.Logic;

namespace SlingoLib
{
    public class Settings
    {
        public TeamSettings Team1 { get; }
        public TeamSettings Team2 { get; }
        public TeamSettings StartingTeam { get; }
        
        public int WordSize { get; }
        
        /// <summary> Seconds available to come up with the next word entry. </summary>
        public int Timeout { get; }

        /// <summary>
        /// The number of times a SLINGO needs to be achieved before the game ends.
        /// </summary>
        public int Rounds { get; }
        

        public Settings(TeamSettings team1, TeamSettings team2, TeamSettings startingTeam, int wordSize, int timeout, int rounds)
        {
            if (startingTeam != team1 && startingTeam != team2)
            {
                throw new ArgumentException("The starting team must be either team 1 or team 2");
            }

            Team1 = team1;
            Team2 = team2;
            StartingTeam= startingTeam;
            WordSize = wordSize;
            Timeout = timeout;
            Rounds = rounds;
        }
    }
}
