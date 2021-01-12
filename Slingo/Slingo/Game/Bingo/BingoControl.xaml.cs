using System.Reactive.Disposables;
using ReactiveUI;

namespace Slingo.Game.Bingo
{
    /// <summary>
    /// Interaction logic for BingoControl.xaml
    /// </summary>
    public partial class BingoControl : ReactiveUserControl<BingoViewModel>
    {
        public BingoControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.FlattendMatrix,
                        view => view.ListView.ItemsSource)
                    .DisposeWith(dispose);
            });
        }
    }
}
