using System.Reactive.Disposables;
using ReactiveUI;

namespace Slingo.Game.Word
{
    /// <summary>
    /// Interaction logic for WordGameControl.xaml
    /// </summary>
    public partial class GameControl : ReactiveUserControl<GameViewModel>
    {
        public GameControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.Bind(ViewModel,
                        vm => vm.Team1ViewModel,
                        view => view.Team1Control.ViewModel)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.Team2ViewModel,
                        view => view.Team2Control.ViewModel)
                    .DisposeWith(dispose);
            });

        }
    }
}
