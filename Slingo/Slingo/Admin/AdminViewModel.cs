using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using Slingo.Admin.Setup;
using Slingo.Admin.WordGameControl;

namespace Slingo.Admin
{
    public class AdminViewModel : ReactiveObject
    {
        private SetupViewModel _setupViewModel;
        public ReactiveObject SelectedViewModel { get; private set; }
        
        public AdminViewModel()
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
