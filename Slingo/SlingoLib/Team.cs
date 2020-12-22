using System;
using System.Collections.Generic;
using System.Text;

namespace SlingoLib
{
    public class Team
    {
        public string Name { get; }
        public string Player1 { get; }
        public string Player2 { get; }

        public Team(string name, string player1, string player2)
        {
            Name = name;
            Player1 = player1;
            Player2 = player2;
        }
    }
}
