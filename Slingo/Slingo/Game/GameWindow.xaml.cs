using System.Reactive.Disposables;
using ReactiveUI;

namespace Slingo.Game
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : ReactiveWindow<GameWindowViewModel>
    {
        public GameWindow()
        {
            InitializeComponent();
            
            this.WhenActivated((dispose) =>
            {
                this.Bind(ViewModel,
                    vm => vm.SelectedViewModel,
                    view => view.ViewModelViewHost.ViewModel)
                    .DisposeWith(dispose);
            });

        }
    }
}
