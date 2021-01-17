using System.Reactive.Disposables;
using System.Windows;
using ReactiveUI;

namespace Slingo.Admin.Word
{
    /// <summary>
    /// Interaction logic for InputControl.xaml
    /// </summary>
    public partial class InputControl : ReactiveUserControl<InputViewModel>
    {
        public InputControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.SelectedViewModel,
                        view => view.ViewModelViewHost.ViewModel)
                    .DisposeWith(dispose);
            });
        }
    }
}
