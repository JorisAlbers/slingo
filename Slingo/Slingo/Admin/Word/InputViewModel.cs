using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using Slingo.Game.Score;
using Slingo.Game.Word;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic.Word;
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
        
        [Reactive] public GameSection GameSectionWithFocus { get; private set; }

        public InputViewModel(GameState gameState, WordRepository wordRepository, Settings settings,
            WordGameViewModel wordGameViewModel)
        {
            _wordRepository = wordRepository;
            _settings = settings;
            _words = _wordRepository.Deserialize(settings.WordSize);
            _random = new Random();

            BingoSetupViewModel1 = new BingoSetupViewModel(gameState.Team1);
            BingoSetupViewModel2 = new BingoSetupViewModel(gameState.Team2);
            WordInputViewModel = new WordInputViewModel(_words, settings, _random, wordGameViewModel);

            FocusTeam1 = ReactiveCommand.Create(() => Unit.Default);
            FocusTeam2 = ReactiveCommand.Create(() => Unit.Default);
            FocusBingoCard = ReactiveCommand.Create(() => Unit.Default);
            FocusWordGame = ReactiveCommand.Create(() => Unit.Default);

            FocusTeam1.Subscribe(x =>
            {
                TeamWithFocus = 1;
                if(GameSectionWithFocus == GameSection.Bingo)
                {
                    SelectedViewModel = BingoSetupViewModel1;
                }

            });
            
            FocusTeam2.Subscribe(x =>
            {
                TeamWithFocus = 2;
                if (GameSectionWithFocus == GameSection.Bingo)
                {
                    SelectedViewModel = BingoSetupViewModel2;
                }
            });

            FocusBingoCard.Subscribe(x =>
            {
                GameSectionWithFocus = GameSection.Bingo;
                if (WordInputViewModel.StateInfo != null && WordInputViewModel.StateInfo.State == WordGameState.Won)
                {
                    WordInputViewModel.Clear();
                }

                if (TeamWithFocus == 1)
                {
                    SelectedViewModel = BingoSetupViewModel1;
                    return;
                }

                SelectedViewModel = BingoSetupViewModel2;
            });

            FocusWordGame.Subscribe(x =>
            {
                GameSectionWithFocus = GameSection.Word;
                SelectedViewModel = WordInputViewModel;
            });

            SelectedViewModel = BingoSetupViewModel1;
        }
        
    }

    public enum GameSection
    {
        Bingo,
        Word
    }
}
