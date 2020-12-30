using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo
{
    public class ScoreboardViewModel : ReactiveObject
    {
        public string TeamName { get; }
        
        [Reactive] public int Score { get; private set; }
        [Reactive] public bool IsActiveTeam { get; set; }
        public HorizontalAlignment HorizontalPosition { get;}

        public ScoreboardViewModel(string teamName, HorizontalAlignment horizontalPosition)
        {
            TeamName = teamName;
            HorizontalPosition = horizontalPosition;
        }
    }
}
