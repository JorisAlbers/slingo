using System.Reactive.Disposables;
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
                        vm => vm.Word,
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
                        vm => vm.NextWord,
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
            });
        }
    }
}
