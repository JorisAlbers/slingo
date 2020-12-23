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
    /// Interaction logic for WordGameRowControl.xaml
    /// </summary>
    public partial class WordGameRowControl : ReactiveUserControl<WordGameRowViewModel>
    {
        public WordGameRowControl()
        {
            InitializeComponent();
            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.Letters,
                        view => view.RowListView.ItemsSource)
                    .DisposeWith(dispose);
            });
        }
    }
}
 