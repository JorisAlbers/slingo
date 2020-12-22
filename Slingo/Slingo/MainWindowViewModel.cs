using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Setup;

namespace Slingo
{
    public class MainWindowViewModel : ReactiveObject
    {
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }


        public MainWindowViewModel()
        {
            SelectedViewModel = new SetupViewModel();
        }
        
    }
}
