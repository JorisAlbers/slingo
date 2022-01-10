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
using MaterialDesignThemes.Wpf;
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

                this.OneWayBind(ViewModel,
                        vm => vm.GreenBall1,
                        view => view.GreenBallEllipse1.Visibility,
                        (b)=> b ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.GreenBall2,
                        view => view.GreenBallEllipse2.Visibility,
                        (b) => b ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.GreenBall3,
                        view => view.GreenBallEllipse3.Visibility,
                        (b) => b ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                    vm => vm.ActiveSceneContainer.Scene,
                    view => view.ActiveSceneTextBlock.Text)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                    vm => vm.OnAir,
                    view => view.OnAirIcon.Visibility,
                    x=> x ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.OnAir,
                        view => view.OffAirIcon.Visibility,
                        x => x ? Visibility.Collapsed : Visibility.Visible)
                    .DisposeWith(dispose);


            });

        }
    }
}
