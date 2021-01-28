using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;
using ReactiveUI;

namespace Slingo.Game.Bingo
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
                this.OneWayBind(ViewModel,
                        vm => vm.Text,
                        view => view.TextTextBlock.Text)
                    .DisposeWith(dispose);

                this.WhenAnyValue(x => x.ActualWidth).Subscribe(x => ViewModel.Width = x);
                this.WhenAnyValue(x => x.ActualHeight).Subscribe(x => ViewModel.Height = x);


                this.OneWayBind(ViewModel,
                    vm => vm.State,
                    view => view.PrimaryColorGradientStop.Color,
                    (state) =>
                    {
                        switch (state)
                        {
                            case BallState.Normal:
                                return Color.FromRgb(60, 86, 173);
                            case BallState.Filled:
                                return Color.FromRgb(209, 224, 69);
                            case BallState.Won:
                                return Color.FromRgb(223, 113, 62);
                            default:
                                throw new ArgumentOutOfRangeException(nameof(state), state, null);
                        }
                    }).DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                    vm => vm.State,
                    view => view.SecondaryColorGradientStop.Color,
                    (state) =>
                    {
                        switch (state)
                        {
                            case BallState.Normal:
                                return Color.FromRgb(0, 0, 0);
                            case BallState.Filled:
                                return Color.FromRgb(159, 120, 0);
                            case BallState.Won:
                                return Color.FromRgb(69, 16, 0);
                            default:
                                throw new ArgumentOutOfRangeException(nameof(state), state, null);
                        }
                    }).DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                    vm => vm.State,
                    view => view.TextTextBlock.Visibility,
                    (state) =>
                    {
                        if (state == BallState.Filled)
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
                        vm => vm.ShowPartlyFilledIndex,
                        view => view.Fill5.Visibility,
                        (index) => index == 5 ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ShowPartlyFilledIndex,
                        view => view.Fill6.Visibility,
                        (index) => index == 6 ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.ShowPartlyFilledIndex,
                        view => view.Fill7.Visibility,
                        (index) => index == 7 ? Visibility.Visible : Visibility.Collapsed)
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

                this.OneWayBind(ViewModel,
                    vm => vm.ShouldFlash,
                    view => view.FlashEllipse.IsEnabled,
                    (flash) => !flash)
                    .DisposeWith(dispose);
            });
        }
    }
}
