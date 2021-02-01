﻿using System;
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
    /// Interaction logic for BingoAdminPanel.xaml
    /// </summary>
    public partial class BingoAdminPanel : ReactiveUserControl<BingoAdminPanelViewModel>
    {
        public BingoAdminPanel()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.Bind(ViewModel,
                        vm => vm.SelectedViewModel,
                        view => view.ViewModelViewHost.ViewModel)
                    .DisposeWith(dispose);
                
            });
        }
    }
}
