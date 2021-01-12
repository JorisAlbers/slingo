using System.Reactive.Disposables;
using ReactiveUI;

namespace Slingo.Game.Word
{
    /// <summary>
    /// Interaction logic for WordGameControl.xaml
    /// </summary>
    public partial class WordGameControl : ReactiveUserControl<WordGameViewModel>
    {
        public WordGameControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.BoardViewModel,
                        view => view.BoardGameViewModelHost.ViewModel)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ScoreBoardTeam1,
                        view => view.Team1ScoreBoard.ViewModel)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ScoreBoardTeam2,
                        view => view.Team2ScoreBoard.ViewModel)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ActiveTeamName,
                        view => view.TeamNameTextBlock.Text)
                    .DisposeWith(dispose);
            });
        }
    }
}
