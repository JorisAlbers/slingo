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

namespace Slingo.Bingo
{
    /// <summary>
    /// Interaction logic for BallControl.xaml
    /// </summary>
    public partial class BallControl : ReactiveUserControl<BingoBallViewModel>
    {
        public BallControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.Bind(ViewModel,
                        vm => vm.Number,
                        view => view.NumberTextBlock.Text)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                    vm => vm.IsFilled,
                    view => view.PrimaryColorGradientStop.Color,
                    (isFilled) =>
                    {
                        if (isFilled)
                        {
                            return Color.FromRgb(209, 224, 69);
                        }
                        else
                        {
                            return Color.FromRgb(60, 86, 173);
                        }
                    }).DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                    vm => vm.IsFilled,
                    view => view.SecondaryColorGradientStop.Color,
                    (isFilled) =>
                    {
                        if (isFilled)
                        {
                            return Color.FromRgb(159, 120, 0);
                        }
                        else
                        {
                            return Color.FromRgb(0,0,0);
                        }
                    }).DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                    vm => vm.IsFilled,
                    view => view.NumberTextBlock.Visibility,
                    (isFilled) =>
                    {
                        if (isFilled)
                        {
                            return Visibility.Collapsed;
                        }
                        else
                        {
                            return Visibility.Visible;
                        }
                    }).DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ShowPartlyFilledIndex,
                        view => view.Fill1.Visibility,
                        (index) => index == 1 ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ShowPartlyFilledIndex,
                        view => view.Fill2.Visibility,
                        (index) => index == 2 ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ShowPartlyFilledIndex,
                        view => view.Fill3.Visibility,
                        (index) => index == 3 ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ShowPartlyFilledIndex,
                        view => view.Fill4.Visibility,
                        (index) => index == 4 ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.IsMatchPoint,
                        view => view.TopMatchpointRectangle.Visibility,
                        (isMatchPoint) => isMatchPoint? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.IsMatchPoint,
                        view => view.BottomMatchpointRectangle.Visibility,
                        (isMatchPoint) => isMatchPoint ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

            });
        }
    }
}
