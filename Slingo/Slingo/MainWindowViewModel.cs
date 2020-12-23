using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Slingo.Admin.Setup;
using Slingo.Admin.WordGameControl;
using Slingo.WordGame;
using Splat;

namespace Slingo
{
    public class MainWindowViewModel : ReactiveObject
    {
        private AdminViewModel _adminViewModel;
        private AdminWindow _adminWindow;

        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        
        public MainWindowViewModel()
        {
            _adminViewModel = new AdminViewModel();
            var view = Locator.Current.GetService<IViewFor<AdminViewModel>>();
            var window = view as ReactiveWindow<AdminViewModel>;
            window.ViewModel = _adminViewModel;
            window.Show();
        }
        
    }
}
