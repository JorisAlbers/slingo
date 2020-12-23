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
using Slingo.Input;

namespace Slingo.WordGame
{
    /// <summary>
    /// Interaction logic for InputControl.xaml
    /// </summary>
    public partial class InputControl : ReactiveUserControl<InputViewModel>
    {
        public InputControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.Bind(ViewModel,
                        vm => vm.Word,
                        view => view.WordTextBox.Text)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.WordIsAccepted,
                        view => view.IsAcceptedCheckBox.IsChecked)
                    .DisposeWith(dispose);
            });
        }
    }
}
