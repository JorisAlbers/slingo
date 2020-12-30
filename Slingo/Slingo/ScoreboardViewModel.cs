using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo
{
    public class ScoreboardViewModel : ReactiveObject
    {
        public string TeamName { get; }
        
        [Reactive] public int Score { get; private set; }
        [Reactive] public bool IsActiveTeam { get; set; }

        public ScoreboardViewModel(string teamName)
        {
            TeamName = teamName;
        }
    }
}
