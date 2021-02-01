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

                this.BindCommand(ViewModel,
                        vm => vm.FocusTeam1,
                        view => view.FocusTeam1Button)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.FocusTeam2,
                        view => view.FocusTeam2Button)
                    .DisposeWith(dispose);

            });
        }
    }
}
