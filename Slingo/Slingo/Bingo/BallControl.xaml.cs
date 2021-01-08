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
            });
        }
    }
}
