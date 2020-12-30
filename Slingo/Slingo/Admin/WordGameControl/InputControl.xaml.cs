using System.Reactive.Disposables;
using System.Windows;
using ReactiveUI;

namespace Slingo.Admin.WordGameControl
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
                this.Bind(ViewModel,
                        vm => vm.WordInputtedByUser,
                        view => view.WordTextBox.Text)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.Accept,
                        view => view.AcceptButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.Reject,
                        view => view.RejectButton)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.CandidateWord,
                        view => view.NextWordTextBlock.Text)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.GenerateWord,
                        view => view.GenerateNewWordButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.StartGame,
                        view => view.StartNewGameButton)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.CurrentWord,
                        view => view.CurrentWordTextBlock.Text)
                    .DisposeWith(dispose);

                this.Hide.Checked += (sender, args) => CurrentWordTextBlock.Visibility = Visibility.Hidden;
                this.Hide.Unchecked += (sender, args) => CurrentWordTextBlock.Visibility = Visibility.Visible;
            });
        }
    }
}
