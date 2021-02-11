﻿using System;
using System.Collections.Generic;
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
        
        public async Task StartWordGame(string word, int activeTeamIndex)
        {
            _wordSize = word.Length;
            _wordGame = new WordGame(new WordPuzzle(word), activeTeamIndex);
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
        public async Task<WordGameState> AcceptWord()
        {
            var result = _wordGame.Solve(_activeWord);
            await BoardViewModel.AcceptWord(result);

            if (_wordGame.State == WordGameState.Won)
            {
                _audioPlaybackEngine.PlaySound(_winSound);
                if (_wordGame.ActiveTeamIndex == 0)
                {
                    _state.Team1.Score += 50;
                }
                else
                {
                    _state.Team2.Score += 50;
                }
                return WordGameState.Won;
            }
            
            if (_wordGame.State == WordGameState.SwitchTeam)
            {
                if (_wordGame.ActiveTeamIndex == 0)
                {
                    _state.Team1.IsActiveTeam = true;
                    _state.Team2.IsActiveTeam = false;
                }
                else
                {
                    _state.Team1.IsActiveTeam = false;
                    _state.Team2.IsActiveTeam = true;
                }
            }
            else if (_wordGame.State == WordGameState.Ongoing)
            {
                await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            }
            
            return WordGameState.Ongoing;
        }

        /// <summary>
        /// Reject the word that was previously set
        /// </summary>
        public async Task RejectWord()
        {
            _wordGame.Reject();
            if (_wordGame.State == WordGameState.SwitchTeam)
            {
                _audioPlaybackEngine.PlaySound(_rejectSound);
            }
            else
            {
                await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            }
        }

        public async Task TimeOut()
        {
            _audioPlaybackEngine.PlaySound(_timeOutSound);
            await RejectWord();
        }
        
        public async Task AddRow()
        {
            // TODO:// Play switch team sound
            await BoardViewModel.AddAdditionalRow();
            await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            // TODO add bonus letter sound
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
    }
}
