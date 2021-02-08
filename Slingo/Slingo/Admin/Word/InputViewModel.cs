using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Serialization;

namespace Slingo.Admin.Word
{
    public class InputViewModel : ReactiveObject
    {
        private readonly WordRepository _wordRepository;
        private readonly Settings _settings;
        private readonly Random _random;
        private readonly List<string> _words;
        
        public BingoSetupViewModel BingoSetupViewModel1 { get;  }
        public BingoSetupViewModel BingoSetupViewModel2 { get;  }
        [Reactive] public WordInputViewModel WordInputViewModel { get; private set; }
        [Reactive] public ReactiveObject SelectedViewModel { get; private set; }

        public ReactiveCommand<Unit, Unit> FocusTeam1 { get; }
        public ReactiveCommand<Unit, Unit> FocusTeam2 { get; }
        public ReactiveCommand<Unit, Unit> FocusBingoCard { get; }
        public ReactiveCommand<Unit, Unit> FocusWordGame { get; }

        [Reactive] public int TeamWithFocus { get; private set; } = 1;
        
        public InputViewModel(WordRepository wordRepository, Settings settings, AudioPlaybackEngine audioPlaybackEngine)
        {
            _wordRepository = wordRepository;
            _settings = settings;
            _words = _wordRepository.Deserialize(settings.WordSize);
            _random = new Random();

            BingoSetupViewModel1 = new BingoSetupViewModel();
            BingoSetupViewModel2 = new BingoSetupViewModel();
            WordInputViewModel = new WordInputViewModel(_words, settings, _random, audioPlaybackEngine);

            FocusTeam1 = ReactiveCommand.Create(() => Unit.Default);
            FocusTeam2 = ReactiveCommand.Create(() => Unit.Default);
            FocusBingoCard = ReactiveCommand.Create(() => Unit.Default);
            FocusWordGame = ReactiveCommand.Create(() => Unit.Default);

            FocusTeam1.Subscribe(x =>
            {
                TeamWithFocus = 1;
                SelectedViewModel = BingoSetupViewModel1;
            });
            
            FocusTeam2.Subscribe(x =>
            {
                TeamWithFocus = 2;
                SelectedViewModel = BingoSetupViewModel2;
            });

            FocusBingoCard.Subscribe(x =>
            {
                if (TeamWithFocus == 1)
                {
                    SelectedViewModel = BingoSetupViewModel1;
                    return;
                }

                SelectedViewModel = BingoSetupViewModel2;
            });

            FocusWordGame.Subscribe(x =>
            {
                SelectedViewModel = WordInputViewModel;
            });

            SelectedViewModel = BingoSetupViewModel1;
        }
        
    }
}
