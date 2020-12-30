using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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

        public ReactiveCommand<Unit,Unit> CountDownStarted { get; } // TODO move to model class

        public GameWindowViewModel()
        {
            CountDownStarted = ReactiveCommand.Create(() => new Unit());
        }

        public async void StartGame(Settings settings, string word, AudioPlaybackEngine audioPlaybackEngine)
        {
            _settings = settings;
            // TODO The next lines should all move to the wordGameViewModel.
            // THis should also keep track of the scores etc, next to the board.
            _wordGameViewModel = new WordGameViewModel(settings,word,audioPlaybackEngine);
            SelectedViewModel = _wordGameViewModel;
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }


        public async void AcceptWord()
        {
            await _wordGameViewModel.AcceptWord();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        public async void RejectWord()
        {
            await _wordGameViewModel.RejectWord();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
        public void SetWord(string word)
        {
            _wordGameViewModel.SetWord(word);
        }

        public async void TimeOut()
        {
            await _wordGameViewModel.TimeOut();
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }
        }
    }
}
