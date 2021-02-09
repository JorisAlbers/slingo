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
                
                this.OneWayBind(ViewModel,
                        vm => vm.TeamWithFocus,
                        view => view.TeamWithFocusTextBlock.Text)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.FocusTeam1,
                        view => view.FocusTeam1Button)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.FocusTeam2,
                        view => view.FocusTeam2Button)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.FocusBingoCard,
                        view => view.FocusBingoCardButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.FocusWordGame,
                        view => view.FocusWordGameButton)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.TeamWithFocus,
                        view => view.TeamWithFocusTextBlock.Text)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.GameSectionWithFocus,
                        view => view.GameModeFocusTextBlock.Text,
                        (section)=> section.ToString())
                    .DisposeWith(dispose);

            });
        }
    }
}
