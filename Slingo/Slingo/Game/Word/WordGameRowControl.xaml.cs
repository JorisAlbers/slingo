using System.Reactive.Disposables;
using ReactiveUI;

namespace Slingo.Game.Word
{
    /// <summary>
    /// Interaction logic for WordGameRowControl.xaml
    /// </summary>
    public partial class WordGameRowControl : ReactiveUserControl<WordGameRowViewModel>
    {
        public WordGameRowControl()
        {
            InitializeComponent();
            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.Letters,
                        view => view.RowListView.ItemsSource)
                    .DisposeWith(dispose);
            });
        }
    }
}
 