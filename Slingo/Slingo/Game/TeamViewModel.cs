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
using Slingo.Sound;
using SlingoLib;

namespace Slingo.Game
{
    public class TeamViewModel : ReactiveObject
    {
        private readonly int _teamIndex;
        private readonly BingoCardSettings _bingoCardSettings;
        private readonly Random _random;
        private readonly AudioPlaybackEngine _audioPlaybackEngine;
        public ScoreboardViewModel Scoreboard1 { get; }
        public ScoreboardViewModel Scoreboard2 { get; }
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        [Reactive] public BoardViewModel BoardViewModel { get; private set; }
        [Reactive] public BingoViewModel BingoViewModel { get; private set; }

        public string TeamName => $"TEAM {_teamIndex +1}";

        public TeamViewModel(int teamIndex, ScoreboardViewModel scoreboard1, ScoreboardViewModel scoreboard2, BingoCardSettings bingoCardSettings, Random random, AudioPlaybackEngine audioPlaybackEngine)
        {
            _teamIndex = teamIndex;
            _bingoCardSettings = bingoCardSettings;
            _random = random;
            _audioPlaybackEngine = audioPlaybackEngine;
            Scoreboard1 = scoreboard1;
            Scoreboard2 = scoreboard2;

            CreateNewBingoCard();
            SelectedViewModel = BingoViewModel;
        }

        public void CreateNewBingoCard()
        {
            BingoViewModel = new BingoViewModel(_bingoCardSettings, _random, _audioPlaybackEngine);
        }

    }
}
