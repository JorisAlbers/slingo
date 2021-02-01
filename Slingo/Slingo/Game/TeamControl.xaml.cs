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

namespace Slingo.Game
{
    /// <summary>
    /// Interaction logic for TeamControl.xaml
    /// </summary>
    public partial class TeamControl : ReactiveUserControl<TeamViewModel>
    {
        public TeamControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.SelectedViewModel,
                        view => view.ViewModelHost.ViewModel)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.Scoreboard1,
                        view => view.Team1ScoreBoard.ViewModel)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.Scoreboard2,
                        view => view.Team2ScoreBoard.ViewModel)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.TeamName,
                        view => view.TeamNameTextBlock.Text)
                    .DisposeWith(dispose);
            });

        }
    }
}
