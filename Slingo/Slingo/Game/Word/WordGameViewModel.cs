using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Word;
using Slingo.Game.Score;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic.Word;

namespace Slingo.Game.Word
{
    public class WordGameViewModel : ReactiveObject
    {
        private readonly GameState _state;
        private readonly AudioPlaybackEngine _audioPlaybackEngine;
        private WordGame _wordGame;
        private string _activeWord;
        private CachedSound _timeOutSound, _winSound, _rejectSound, _newLetterAppearsSound;
        private int _wordSize;

        [Reactive] public BoardViewModel BoardViewModel { get; private set; }

        public WordGameViewModel(GameState state, AudioPlaybackEngine audioPlaybackEngine)
        {
            _state = state;
            _audioPlaybackEngine = audioPlaybackEngine;
            _timeOutSound = new CachedSound(@"Resources\Sounds\WordGame\timeout.wav");
            _winSound = new CachedSound(@"Resources\Sounds\WordGame\win.wav");
            _rejectSound = new CachedSound(@"Resources\Sounds\WordGame\rejected.wav");
            _newLetterAppearsSound = new CachedSound(@"Resources\Sounds\WordGame\first_letter_appears.wav");
        }
        
        public async Task StartWordGame(string word)
        {
            _wordSize = word.Length;
            _wordGame = new WordGame(new WordPuzzle(word), _state.Team1.IsActiveTeam ? 0 : 1);
            BoardViewModel = new BoardViewModel(word.Length, _audioPlaybackEngine);
           
            _audioPlaybackEngine.PlaySound(_newLetterAppearsSound);
            await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
        }

        public void SetWord(string word)
        {
            if (word.Length < _wordSize)
            {
                word = word + new string('.', _wordSize - word.Length);
            }

            if (word.Length > _wordSize)
            {
                word = word.Substring(0, _wordSize);
            }


            _activeWord = word;

            BoardViewModel.SetWord(word);
        }

        /// <summary>
        /// Accept the word that was previously set
        /// </summary>
        public async Task<WordGameStateInfo> AcceptWord()
        {
            var result = _wordGame.Solve(_activeWord);
            await BoardViewModel.AcceptWord(result);

            if (_wordGame.State.State == WordGameState.Won)
            {
                _audioPlaybackEngine.PlaySound(_winSound);
                if (_wordGame.ActiveTeamIndex == 0)
                {
                    _state.Team1.Score += 25;
                }
                else
                {
                    _state.Team2.Score += 25;
                }
            }
            else if (_wordGame.State.State == WordGameState.SwitchTeam)
            {
                _state.SwitchActiveTeam();
            }
            else if (_wordGame.State.State == WordGameState.Ongoing)
            {
                await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            }

            return _wordGame.State;
        }
        
        /// <summary>
        /// Reject the word that was previously set
        /// </summary>
        public async Task<WordGameStateInfo> RejectWord()
        {
            _wordGame.Reject();
            if (_wordGame.State.State == WordGameState.SwitchTeam)
            {
                _audioPlaybackEngine.PlaySound(_rejectSound);
                _state.SwitchActiveTeam();
            }
            else
            {
                await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            }

            return _wordGame.State;
        }

        public async Task<WordGameStateInfo> TimeOut()
        {
            _audioPlaybackEngine.PlaySound(_timeOutSound);
            _wordGame.TimeOut();
            if (_wordGame.State.State == WordGameState.SwitchTeam)
            {
                _state.SwitchActiveTeam();
            }

            return _wordGame.State;
        }
        
        public async Task AddRow()
        {
            // TODO:// Play switch team sound
            await BoardViewModel.AddAdditionalRow();
            await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
        }

        public async Task AddBonusLetter()
        {
            // TODO: Play bonus letter appears sound
            char letter = _wordGame.AddBonusLetter(out int index);
            await BoardViewModel.AddBonusLetter(letter, index);
        }

        public void Clear()
        {
            BoardViewModel = null;
        }

        public async Task ShowWord(string currentWord)
        {
            await BoardViewModel.AddAdditionalRow();
            await Task.Delay(1000);
            await BoardViewModel.ShowWord(currentWord);
        }
    }
}
