using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Slingo.Sound;
using Slingo.WordGame;
using SlingoLib;
using SlingoLib.Logic;
using SlingoLib.Serialization;
using Splat;

namespace Slingo.Game
{
    public class GameWindowViewModel : ReactiveObject
    {
        private WordGameViewModel _wordGameViewModel;
        private Settings _settings;
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        

        public void StartGame(Settings settings, string word, AudioPlaybackEngine audioPlaybackEngine)
        {
            _settings = settings;
            // TODO The next lines should all move to the wordGameViewModel.
            // THis should also keep track of the scores etc, next to the board.
            _wordGameViewModel = new WordGameViewModel(settings,word,audioPlaybackEngine);
            SelectedViewModel = _wordGameViewModel;
        }


        public void AcceptWord()
        {
            _wordGameViewModel.AcceptWord();
        }
        public void RejectWord()
        {
            _wordGameViewModel.RejectWord();
        }
        public void SetWord(string word)
        {
            _wordGameViewModel.SetWord(word);
        }
    }
}
