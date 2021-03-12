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

namespace Slingo.Admin.Bingo
{
    /// <summary>
    /// Interaction logic for BingoSetupControl.xaml
    /// </summary>
    public partial class BingoSetupControl : ReactiveUserControl<BingoSetupViewModel>
    {
        public BingoSetupControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.BindCommand(ViewModel,
                        vm => vm.Initialize,
                        view => view.InitializeButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.ClearBalls,
                        view => view.ClearBallsButton)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                    vm => vm.NumberString,
                    view => view.BallNumberTextBox.Text)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.BallSubmitted,
                        view => view.SubmitBallButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.GreenBallSubmitted,
                        view => view.GreenBallButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.RedBallSubmitted,
                        view => view.RedBallButton)
                    .DisposeWith(dispose);
            });
        }
    }
}
