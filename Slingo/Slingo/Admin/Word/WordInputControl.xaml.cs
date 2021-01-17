using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;

namespace Slingo.Admin.Word
{
    /// <summary>
    /// Interaction logic for WordInputControl.xaml
    /// </summary>
    public partial class WordInputControl : ReactiveUserControl<WordInputViewModel>
    {
        public WordInputControl()
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

                this.BindCommand(ViewModel,
                        vm => vm.AddRowAndSwitchTeam,
                        view => view.AddRowAndSwitchTeamButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.AddBonusLetter,
                        view => view.AddBonusLetterButton)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.CurrentWord,
                        view => view.CurrentWordTextBlock.Text)
                    .DisposeWith(dispose);

                this.Hide.Checked += (sender, args) => CurrentWordTextBlock.Visibility = Visibility.Hidden;
                this.Hide.Unchecked += (sender, args) => CurrentWordTextBlock.Visibility = Visibility.Visible;

                this.OneWayBind(ViewModel,
                        vm => vm.TimeLeftBeforeTimeOut,
                        view => view.TimeLeftTextBlock.Text)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.AutoTimeOut,
                        view => view.AutoTimeOutCheckBox.IsChecked)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.TimeOut,
                        view => view.ForceTimeOutButton)
                    .DisposeWith(dispose);
            });
        }
    }
}
