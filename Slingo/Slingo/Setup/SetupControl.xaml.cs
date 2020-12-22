using System.Reactive.Disposables;
using ReactiveUI;

namespace Slingo.Setup
{
    /// <summary>
    /// Interaction logic for SetupControl.xaml
    /// </summary>
    public partial class SetupControl : ReactiveUserControl<SetupViewModel>
    {
        public SetupControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.Bind(ViewModel,
                        vm => vm.TeamA,
                        view => view.TeamAViewHost.ViewModel)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.TeamB,
                        view => view.TeamBViewHost.ViewModel)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                    vm => vm.Start,
                    view => view.StartButton)
                    .DisposeWith(dispose);

            });

        }
    }
}
