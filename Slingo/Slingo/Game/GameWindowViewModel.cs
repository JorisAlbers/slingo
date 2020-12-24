using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Slingo.WordGame;
using SlingoLib;
using Splat;

namespace Slingo.Game
{
    public class GameWindowViewModel : ReactiveObject
    {
        private BoardViewModel _boardViewModel;
        private Settings _settings;
        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        
        public GameWindowViewModel()
        {
        }

        public void StartGame(Settings settings)
        {
            _settings = settings;
            //WordGameViewModel viewmodel = new WordGameViewModel(settings);

            SlingoLib.Logic.WordGame gameLogic = new SlingoLib.Logic.WordGame("noten");
            
            _boardViewModel = new BoardViewModel(settings.WordSize);
            SelectedViewModel = _boardViewModel;
            _boardViewModel.StartNextAttempt("n....");
        }

        public void SetWord(string word)
        {
            if (word.Length < _settings.WordSize)
            {
                word = word + new string('.', _settings.WordSize - word.Length);
            }

            if (word.Length > _settings.WordSize)
            {
                word = word.Substring(0, _settings.WordSize);
            }

            _boardViewModel.SetWord(word);
        }
    }
}
