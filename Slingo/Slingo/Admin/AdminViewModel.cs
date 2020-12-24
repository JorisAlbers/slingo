using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Setup;
using Slingo.Admin.WordGameControl;
using Slingo.Game;
using SlingoLib.Serialization;
using Splat;

namespace Slingo.Admin
{
    public class AdminViewModel : ReactiveObject
    {
        private SetupViewModel _setupViewModel;
        private GameWindowViewModel _gameWindowViewModel;
        
        [Reactive] public ReactiveObject SelectedViewModel { get; private set; }
        
        public AdminViewModel()
        {
            _setupViewModel = new SetupViewModel();
            _setupViewModel.Start.Subscribe(settings =>
            {
                _gameWindowViewModel.StartGame(settings);
                InputViewModel inputViewModel = new InputViewModel();
                inputViewModel.WhenAnyValue(x => x.Word).Where(x=>!string.IsNullOrWhiteSpace(x)).Subscribe(onNext => _gameWindowViewModel.SetWord(onNext.ToLower()));
                inputViewModel.Accept.Subscribe(onNext => _gameWindowViewModel.AcceptWord());

                    SelectedViewModel = inputViewModel;
            });

            SelectedViewModel = _setupViewModel;

            _gameWindowViewModel = new GameWindowViewModel(new WordRepository(new FileSystem(), @"Resources\basiswoorden-gekeurd.txt"));
            var view = Locator.Current.GetService<IViewFor<GameWindowViewModel>>();
            var window = view as ReactiveWindow<GameWindowViewModel>;
            window.ViewModel = _gameWindowViewModel;
            window.Show();
        }
        
    }
}
