﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Slingo.WordGame;
using SlingoLib;
using Splat;

namespace Slingo.Game
{
    public class GameWindowViewModel : ReactiveObject
    {
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        
        public GameWindowViewModel()
        {
        }

        public void StartGame(Settings settings)
        {
            //WordGameViewModel viewmodel = new WordGameViewModel(settings);
            BoardViewModel boardViewModel = new BoardViewModel(5);
            SelectedViewModel = boardViewModel;
        }
    }
}