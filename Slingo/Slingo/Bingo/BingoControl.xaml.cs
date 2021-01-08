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
            });
        }
    }
}
