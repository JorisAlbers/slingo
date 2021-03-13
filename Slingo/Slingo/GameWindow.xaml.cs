using System.Reactive.Disposables;
using ReactiveUI;
using Slingo.Game.Word;

namespace Slingo
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : ReactiveWindow<GameViewModel>
    {
        public GameWindow()
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
