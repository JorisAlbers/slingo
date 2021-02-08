using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Game.Bingo;
using Slingo.Game.Score;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic.Word;

namespace Slingo.Game.Word
{
    public class GameViewModel : ReactiveObject
    {
        public ScoreboardViewModel ScoreBoardTeam1 { get; }
        public ScoreboardViewModel ScoreBoardTeam2 { get; }
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        public WordGameViewModel WordGameViewModel { get; set; }
        [Reactive] public TeamViewModel Team1ViewModel { get; private set; }
        [Reactive] public TeamViewModel Team2ViewModel { get; private set; }

        public ReactiveCommand<Unit,Unit> CountDownStarted { get; } // TODO move to model class
        

        public GameViewModel(Settings settings, Team team1, Team team2,  AudioPlaybackEngine audioPlaybackEngine)
        {
            Random random = new Random();
            var audioPlaybackEngine1 = audioPlaybackEngine;

            ScoreBoardTeam1 = new ScoreboardViewModel(team1.Settings.Name, team1.Score, HorizontalAlignment.Left);
            ScoreBoardTeam2 = new ScoreboardViewModel(team2.Settings.Name, team2.Score, HorizontalAlignment.Right);
            WordGameViewModel = new WordGameViewModel(ScoreBoardTeam1, ScoreBoardTeam2, audioPlaybackEngine);

            var bingoCardSettingsTeam1 = new BingoCardSettings(true, settings.ExcludedBallNumbersEven);
            var bingoCardSettingsTeam2 = new BingoCardSettings(false, settings.ExcludedBallNumbersOdd);

            Team1ViewModel = new TeamViewModel(0, ScoreBoardTeam1, ScoreBoardTeam2, WordGameViewModel, bingoCardSettingsTeam1, random, audioPlaybackEngine1);
            Team2ViewModel = new TeamViewModel(1, ScoreBoardTeam1, ScoreBoardTeam2, WordGameViewModel, bingoCardSettingsTeam2, random, audioPlaybackEngine1);
            
            CountDownStarted = ReactiveCommand.Create(() => new Unit());

           

            this.WhenAnyValue(x => x.WordGameViewModel.BoardViewModel).Subscribe(x =>
            {
                WordGameStarted();
            });
        }
        
        public void WordGameStarted()
        {
            Team1ViewModel.FocusWordGame();
            Team2ViewModel.FocusWordGame();
        }

        public void FocusTeam(int i)
        {
            SelectedViewModel = i == 0 ? Team1ViewModel : Team2ViewModel;
        }
    }
}