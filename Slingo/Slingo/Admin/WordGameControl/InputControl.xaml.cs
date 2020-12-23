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

                this.Bind(ViewModel,
                        vm => vm.WordIsAccepted,
                        view => view.IsAcceptedCheckBox.IsChecked)
                    .DisposeWith(dispose);
            });
        }
    }
}
