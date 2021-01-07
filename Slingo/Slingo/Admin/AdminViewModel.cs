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
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Serialization;
using Splat;

namespace Slingo.Admin
{
    public class AdminViewModel : ReactiveObject
    {
        private SetupViewModel _setupViewModel;
        private GameWindowViewModel _gameWindowViewModel;
        private AudioPlaybackEngine _audioPlaybackEngine;

        [Reactive] public ReactiveObject SelectedViewModel { get; private set; }
        
        public AdminViewModel()
        {
            var correctSound = new CachedSound(@"Resources\Sounds\WordGame\correct.wav");
            _audioPlaybackEngine = new AudioPlaybackEngine(correctSound.WaveFormat.SampleRate, correctSound.WaveFormat.Channels);
;            _setupViewModel = new SetupViewModel();
            _setupViewModel.Start.Subscribe(settings =>
            {
                InputViewModel inputViewModel = new InputViewModel(new WordRepository(new FileSystem(), @"Resources\basiswoorden-gekeurd.txt"), settings);
                inputViewModel.StartGame.Subscribe(word=>  _gameWindowViewModel.StartGame(settings, word));
                inputViewModel.WhenAnyValue(x => x.WordInputtedByUser).Where(x=>!string.IsNullOrWhiteSpace(x)).Subscribe(onNext => _gameWindowViewModel.SetWord(WordFormatter.Format(onNext)));
                inputViewModel.Accept.Subscribe(onNext => _gameWindowViewModel.AcceptWord());
                inputViewModel.Reject.Subscribe(onNext => _gameWindowViewModel.RejectWord());
                inputViewModel.TimeOut.Subscribe(onNext => _gameWindowViewModel.TimeOut());
                inputViewModel.AddRowAndSwitchTeam.Subscribe(onNext => _gameWindowViewModel.AddRowAndSwitchTeam());
                inputViewModel.AddBonusLetter.Subscribe(onNext => _gameWindowViewModel.AddBonusLetter());

                _gameWindowViewModel.CountDownStarted.Subscribe(onNext => inputViewModel.StartCountDown());

                SelectedViewModel = inputViewModel;
            });

            SelectedViewModel = _setupViewModel;

            _gameWindowViewModel = new GameWindowViewModel(_audioPlaybackEngine);
            var view = Locator.Current.GetService<IViewFor<GameWindowViewModel>>();
            var window = view as ReactiveWindow<GameWindowViewModel>;
            window.ViewModel = _gameWindowViewModel;
            window.Show();
        }
        
    }
}
