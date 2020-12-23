using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Setup;
using Slingo.Admin.WordGameControl;
using Slingo.WordGame;

namespace Slingo
{
    public class MainWindowViewModel : ReactiveObject
    {
        private SetupViewModel _setupViewModel;
        
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        
        public MainWindowViewModel()
        {
            _setupViewModel = new SetupViewModel();
            _setupViewModel.Start.Subscribe(settings =>
            {
                //WordGameViewModel viewmodel = new WordGameViewModel(settings);
                //BoardViewModel boardViewModel = new BoardViewModel(5);
                InputViewModel inputViewModel = new InputViewModel();
                SelectedViewModel = inputViewModel;
            });
            
            SelectedViewModel = _setupViewModel;
        }
        
    }
}
