using System;
using System.Collections.Generic;
using System.Text;

namespace SlingoLib
{
    public class Team
    {
        public TeamSettings Settings { get; }

        public int Score { get; set; }

        public Team(TeamSettings settings)
        {
            Settings = settings;
        }
    }
}
