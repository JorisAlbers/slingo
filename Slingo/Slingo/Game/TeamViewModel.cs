using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Game.Bingo;
using Slingo.Game.Score;
using Slingo.Game.Word;
using SlingoLib;

namespace Slingo.Game
{
    public class TeamViewModel : ReactiveObject
    {
        private readonly int _teamIndex;
        public ScoreboardViewModel Scoreboard1 { get; }
        public ScoreboardViewModel Scoreboard2 { get; }
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        [Reactive] public BoardViewModel BoardViewModel { get; private set; }
        [Reactive] public BingoViewModel BingoViewModel { get; set; }

        public string TeamName => $"TEAM {_teamIndex +1}";

        public TeamViewModel(int teamIndex, ScoreboardViewModel scoreboard1, ScoreboardViewModel scoreboard2, BingoViewModel bingoViewModel)
        {
            _teamIndex = teamIndex;
            Scoreboard1 = scoreboard1;
            Scoreboard2 = scoreboard2;
            BingoViewModel = bingoViewModel;
            SelectedViewModel = BingoViewModel;
        }

    }
}
