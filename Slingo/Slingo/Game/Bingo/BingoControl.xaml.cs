using System;
using System.Reactive.Disposables;
using System.Windows;
using ReactiveUI;

namespace Slingo.Game.Bingo
{
    /// <summary>
    /// Interaction logic for BingoControl.xaml
    /// </summary>
    public partial class BingoControl : ReactiveUserControl<BingoViewModel>
    {
        public BingoControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.FlattendMatrix,
                        view => view.ListView.ItemsSource)
                    .DisposeWith(dispose);
                
                this.OneWayBind(ViewModel,
                        vm => vm.FlattendMatrixForAnimation,
                        view => view.Canvas.ItemsSource)
                    .DisposeWith(dispose);

                this.WhenAnyValue(x => x.ListView.ActualHeight).Subscribe(x => ViewModel.HeightOfMatrix = x);

                this.OneWayBind(ViewModel,
                    vm => vm.IsAnimating,
                    view => view.ListView.Visibility,
                    (isDropping) => isDropping ? Visibility.Hidden : Visibility.Visible);

                this.OneWayBind(ViewModel,
                    vm => vm.IsAnimating,
                    view => view.Canvas.Visibility,
                    (isDropping) => isDropping ? Visibility.Visible : Visibility.Hidden);
            });
        }
    }
}
