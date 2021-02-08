using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Game.Score;
using Slingo.Sound;
using SlingoLib;
using SlingoLib.Logic.Word;

namespace Slingo.Game.Word
{
    public class WordGameViewModel : ReactiveObject
    {
        private readonly ScoreboardViewModel _scoreBoardTeam1;
        private readonly ScoreboardViewModel _scoreBoardTeam2;
        private readonly AudioPlaybackEngine _audioPlaybackEngine;
        private WordGame _wordGame;
        private string _activeWord;
        private CachedSound _timeOutSound, _winSound, _rejectSound, _newLetterAppearsSound;
        private int _wordSize;

        [Reactive] public BoardViewModel BoardViewModel { get; private set; }

        public WordGameViewModel(ScoreboardViewModel scoreBoardTeam1, ScoreboardViewModel scoreBoardTeam2, AudioPlaybackEngine audioPlaybackEngine)
        {
            _scoreBoardTeam1 = scoreBoardTeam1;
            _scoreBoardTeam2 = scoreBoardTeam2;
            _audioPlaybackEngine = audioPlaybackEngine;
            _timeOutSound = new CachedSound(@"Resources\Sounds\WordGame\timeout.wav");
            _winSound = new CachedSound(@"Resources\Sounds\WordGame\win.wav");
            _rejectSound = new CachedSound(@"Resources\Sounds\WordGame\rejected.wav");
            _newLetterAppearsSound = new CachedSound(@"Resources\Sounds\WordGame\first_letter_appears.wav");
        }
        
        public async Task StartWordGame(string word)
        {
            int activeTeamIndex = 0;
            _wordSize = word.Length;
            _wordGame = new WordGame(new WordPuzzle(word), activeTeamIndex);
            BoardViewModel = new BoardViewModel(word.Length, _audioPlaybackEngine);
           
            _audioPlaybackEngine.PlaySound(_newLetterAppearsSound);
            BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);

            /*if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }*/
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
                    _scoreBoardTeam1.Score += 50;
                }
                else
                {
                    _scoreBoardTeam2.Score += 50;
                }
                return WordGameState.Won;
            }

            if (_wordGame.State == WordGameState.Ongoing)
            {
                await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            }

            /*
            if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }*/

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

            /*if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }*/

        }

        public async Task TimeOut()
        {
            _audioPlaybackEngine.PlaySound(_timeOutSound);
            await RejectWord();

            /*if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }*/
        }

        private void SetActiveTeam(int index)
        {
            if (index == 0)
            {
                _scoreBoardTeam1.IsActiveTeam = true;
                _scoreBoardTeam2.IsActiveTeam = false;
            }
            else
            {
                _scoreBoardTeam2.IsActiveTeam = true;
                _scoreBoardTeam1.IsActiveTeam = false;
            }
        }

        public async Task AddRowAndSwitchTeam()
        {
            // TODO:// Play switch team sound
            await BoardViewModel.AddAdditionalRow();
            SetActiveTeam(_wordGame.ActiveTeamIndex);
            await BoardViewModel.StartNextAttempt(_wordGame.KnownLetters);
            // TODO add bonus letter sound

            /*if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }*/
        }

        public async Task AddBonusLetter()
        {
            // TODO: Play bonus letter appears sound
            char letter = _wordGame.AddBonusLetter(out int index);
            await BoardViewModel.AddBonusLetter(letter, index);

            /*if (await CountDownStarted.CanExecute.FirstAsync()) // TODO move to model class
            {
                await CountDownStarted.Execute();
            }*/
        }
    }
}
