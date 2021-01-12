using System.Reactive.Disposables;
using ReactiveUI;

namespace Slingo.Game.Word
{
    /// <summary>
    /// Interaction logic for BoardControl.xaml
    /// </summary>
    public partial class BoardControl : ReactiveUserControl<BoardViewModel>
    {
        public BoardControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.Rows,
                        view => view.ListView.ItemsSource)
                    .DisposeWith(dispose);
            });
        }
    }
}
