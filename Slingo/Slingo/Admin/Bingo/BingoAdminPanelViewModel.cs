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
        public BingoSetupViewModel SetupViewModelTeam1 { get; }
        public BingoSetupViewModel SetupViewModelTeam2 { get; }
        public BingoInputViewModel BingoInputViewModel { get; }
        
        public ReactiveCommand<Unit,Unit> Forwards { get; }
        public ReactiveCommand<Unit,Unit> Backwards { get; }
        
        public BingoAdminPanelViewModel(BingoCardSettings settingsTeam1, BingoCardSettings settingsTeam2)
        {
            BingoInputViewModel = new BingoInputViewModel();
            SetupViewModelTeam1 = new BingoSetupViewModel(0, settingsTeam1);
            SetupViewModelTeam2 = new BingoSetupViewModel(1, settingsTeam2);

            this.SetupViewModelTeam1.Initialize.Subscribe(x =>
                SelectedViewModel = SetupViewModelTeam2);
            this.SetupViewModelTeam2.Initialize.Subscribe(x =>
                SelectedViewModel = BingoInputViewModel);

            SelectedViewModel = SetupViewModelTeam1;

            Forwards = ReactiveCommand.Create(() => { },  this.WhenAnyValue(x=> x.SelectedViewModel, (vm)=> vm != BingoInputViewModel));
            Backwards = ReactiveCommand.Create(() => { }, this.WhenAnyValue(x=> x.SelectedViewModel, (vm)=> vm != SetupViewModelTeam1));
            
            Forwards.Subscribe(x =>
            {
                if (SelectedViewModel == SetupViewModelTeam1)
                {
                    SelectedViewModel = SetupViewModelTeam2;
                    return;
                }

                if (SelectedViewModel == SetupViewModelTeam2)
                {
                    SelectedViewModel = BingoInputViewModel;
                    return;
                }
            });

            Backwards.Subscribe(x =>
            {
                if (SelectedViewModel == BingoInputViewModel)
                {
                    SelectedViewModel = SetupViewModelTeam2;
                    return;
                }

                if (SelectedViewModel == SetupViewModelTeam2)
                {
                    SelectedViewModel = SetupViewModelTeam1;
                    return;
                }
            });

        }
    }
}
