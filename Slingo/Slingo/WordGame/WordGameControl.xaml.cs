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

namespace Slingo.WordGame
{
    /// <summary>
    /// Interaction logic for WordGameControl.xaml
    /// </summary>
    public partial class WordGameControl : ReactiveUserControl<WordGameViewModel>
    {
        public WordGameControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.BoardViewModel,
                        view => view.BoardGameViewModelHost.ViewModel)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ScoreBoardTeam1,
                        view => view.Team1ScoreBoard.ViewModel)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ScoreBoardTeam2,
                        view => view.Team2ScoreBoard.ViewModel)
                    .DisposeWith(dispose);
            });
        }
    }
}
