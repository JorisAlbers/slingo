using System.Reactive.Disposables;
using ReactiveUI;

namespace Slingo.Admin.Setup
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
               this.BindCommand(ViewModel,
                    vm => vm.Start,
                    view => view.StartButton)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.WordSize,
                        view => view.WordSizeTextBox.Text)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.Rounds,
                        view => view.RoundsTextBox.Text)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.TimeOut,
                        view => view.TimeOutTextBox.Text)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.Team1Starts,
                        view => view.Team1ActiveRadioButton.IsChecked)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.Team2Starts,
                        view => view.Team2ActiveRadioButton.IsChecked)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.AudioOutputDevices,
                        view => view.AudioOutputComboBox.ItemsSource)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.SelectedAudioOutput,
                        view => view.AudioOutputComboBox.SelectedItem)
                    .DisposeWith(dispose);

            });

        }
    }
}
