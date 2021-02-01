using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Admin.Bingo
{
    public class BingoAdminPanelViewModel : ReactiveObject
    {
        [Reactive] public ReactiveObject SelectedViewModel { get; private set; }
        public BingoSetupViewModel SetupViewModel { get; }
        public BingoInputViewModel BingoInputViewModel { get; }

        public BingoAdminPanelViewModel(int teamIndex)
        {
            BingoInputViewModel = new BingoInputViewModel();
            SetupViewModel = new BingoSetupViewModel(teamIndex);

            SelectedViewModel = SetupViewModel;
        }
        
        
    }
}
