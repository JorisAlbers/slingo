using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Game.Score
{
    public class ScoreboardViewModel : ReactiveObject
    {
        public string TeamName { get; }
        
        [Reactive] public int Score { get; set; }
        [Reactive] public bool IsActiveTeam { get; set; }
        public HorizontalAlignment HorizontalPosition { get;}

        public ScoreboardViewModel(string teamName,int score, HorizontalAlignment horizontalPosition)
        {
            TeamName = teamName;
            HorizontalPosition = horizontalPosition;
            Score = score;
        }
    }
}
