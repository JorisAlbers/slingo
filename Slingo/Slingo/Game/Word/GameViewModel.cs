using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Admin.Word;
using Slingo.Game.Score;
using Slingo.Sound;

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
        

        public GameViewModel(Settings settings, GameState state,  AudioPlaybackEngine audioPlaybackEngine, ActiveSceneContainer activeSceneContainer)
        {
            Random random = new Random();
            var audioPlaybackEngine1 = audioPlaybackEngine;

            ScoreBoardTeam1 = new ScoreboardViewModel(state.Team1,  HorizontalAlignment.Left);
            ScoreBoardTeam2 = new ScoreboardViewModel(state.Team2,  HorizontalAlignment.Right);
            WordGameViewModel = new WordGameViewModel(state, audioPlaybackEngine);

            var bingoCardSettingsTeam1 = new BingoCardSettings(true, settings.ExcludedBallNumbersEven);
            var bingoCardSettingsTeam2 = new BingoCardSettings(false, settings.ExcludedBallNumbersOdd);

            Team1ViewModel = new TeamViewModel(0, ScoreBoardTeam1, ScoreBoardTeam2, WordGameViewModel, bingoCardSettingsTeam1, random, audioPlaybackEngine1, activeSceneContainer);
            Team2ViewModel = new TeamViewModel(1, ScoreBoardTeam1, ScoreBoardTeam2, WordGameViewModel, bingoCardSettingsTeam2, random, audioPlaybackEngine1, activeSceneContainer);
            
            CountDownStarted = ReactiveCommand.Create(() => new Unit());

            this.WhenAnyValue(x => x.WordGameViewModel.BoardViewModel).Where(x=>x!=null).Subscribe(x =>
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