using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Slingo.Admin.Bingo;
using Slingo.Game.Bingo;
using Slingo.Game.Word;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic;
using SlingoLib.Serialization;
using Splat;

namespace Slingo.Game
{
    public class GameWindowViewModel : ReactiveObject
    {

        [Reactive] public GameViewModel GameViewModel { get; private set; }
        
        public GameWindowViewModel()
        {
            
        }

        public void StartGame(GameViewModel gameViewModel)
        {
            GameViewModel = gameViewModel;
        }
    }
}
