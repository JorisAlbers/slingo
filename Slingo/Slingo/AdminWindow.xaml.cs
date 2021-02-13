using System.Reactive.Disposables;
using ReactiveUI;
using Slingo.Admin;

namespace Slingo
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : ReactiveWindow<AdminViewModel>
    {
        public AdminWindow()
        {
            InitializeComponent();
            ViewModel = new AdminViewModel();

            this.WhenActivated((dispose) =>
            {
                this.Bind(ViewModel,
                        vm => vm.SelectedAdminViewModel,
                        view => view.ViewModelViewHost.ViewModel)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.GameViewModel,
                        view => view.GameViewModelHost.ViewModel)
                    .DisposeWith(dispose);
            });
        }
    }
}
