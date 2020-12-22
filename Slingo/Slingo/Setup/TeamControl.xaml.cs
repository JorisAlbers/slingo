using ReactiveUI;

namespace Slingo.Setup
{
    /// <summary>
    /// Interaction logic for TeamControl.xaml
    /// </summary>
    public partial class TeamControl : ReactiveUserControl<TeamViewModel>
    {
        public TeamControl()
        {
            InitializeComponent();
            this.WhenActivated(disposableRegistration =>
            {
                this.Bind(ViewModel,
                    vm => vm.Name,
                    view => view.NameTextBlock.Text);

                this.Bind(ViewModel,
                    vm => vm.Player1,
                    view => view.Player1TextBox.Text);

                this.Bind(ViewModel,
                    vm => vm.Player2,
                    view => view.Player2TextBox.Text);

            });
        }
    }
}
