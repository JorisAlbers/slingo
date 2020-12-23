using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Setup;
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
                BoardViewModel boardViewModel = new BoardViewModel(5);
                SelectedViewModel = boardViewModel;
            });
            
            SelectedViewModel = _setupViewModel;
        }
        
    }
}
