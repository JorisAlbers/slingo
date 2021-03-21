using System;
using System.Linq;

namespace SlingoLib.Logic.Word
{
    public class WordGame
    {
        private readonly WordPuzzle _wordPuzzle;
        private int _startingTeamIndex;

        public WordGame(WordPuzzle wordPuzzle, int startingTeamIndex)
        {
            _startingTeamIndex = startingTeamIndex;
            _wordPuzzle = wordPuzzle;
            KnownLetters = wordPuzzle.Word[0] + new string('.', wordPuzzle.Word.Length - 1);
            ActiveTeamIndex = startingTeamIndex;
        }

        public string KnownLetters { get; private set; }

        public int ActiveTeamIndex { get; private set; }

        public int AttemptIndex { get; private set; }

        public WordGameStateInfo State { get; private set; } = new WordGameStateInfo(WordGameState.NotStarted);

        public WordPuzzleEntry Solve(string word)
        {
            ThrowIfGameIsOver();
            AttemptIndex++;

            var puzzleEntry = _wordPuzzle.Solve(word);
            if (puzzleEntry.LetterEntries.All(x => x.State == LetterState.CorrectLocation))
            {
                State = new WordGameStateInfo(WordGameState.Won);
                return puzzleEntry;
            }

            UpdateKnownLetters(puzzleEntry);

            if (AttemptIndex == 6)
            {
                State = new WordGameStateInfo(WordGameState.Lost);
            }
            else if (AttemptIndex == 5) // matrix full. switch teams.
            {
                SwitchTeam(true);
            }
            else
            {
                if (ActiveTeamIndex != _startingTeamIndex)
                {
                    // the attempt made by the other team was not correct.
                    // Switch back the turn to the original team.
                    ActiveTeamIndex = _startingTeamIndex;
                    State = new WordGameStateInfo(WordGameState.SwitchTeam, SwitchTeamFlags.Normal);
                }
                else
                {
                    State = new WordGameStateInfo(WordGameState.Ongoing);
                }

            }

            return puzzleEntry;
        }

        private void UpdateKnownLetters(WordPuzzleEntry entry)
        {
            char[] knownLetters = KnownLetters.ToCharArray();
            for (int i = 0; i < KnownLetters.Length; i++)
            {
                if (knownLetters[i] == '.')
                {
                    if (entry.LetterEntries[i].State == LetterState.CorrectLocation)
                    {
                        knownLetters[i] = entry.LetterEntries[i].Letter;
                    }
                }
            }

            KnownLetters = new string(knownLetters);
        }

        public void Reject()
        {
            ThrowIfGameIsOver();
            SwitchTeam(false);
        }

        public void TimeOut()
        {
            ThrowIfGameIsOver();
            SwitchTeam(false);
        }

        public char AddBonusLetter(out int index)
        {
            char[] knownLetters = KnownLetters.ToCharArray();
            for (int i = 0; i < KnownLetters.Length; i++)
            {
                if (knownLetters[i] == '.')
                {
                    knownLetters[i] = _wordPuzzle.Word[i];
                    KnownLetters = new string(knownLetters);
                    index = i;
                    return _wordPuzzle.Word[i];
                }
            }

            throw new Exception("Word is already known");
        }

        private void ThrowIfGameIsOver()
        {
            if (State == null)
            {
                return;
            }
            if (State.State == WordGameState.Won || State.State == WordGameState.Lost)
            {
                throw new InvalidOperationException("Game is already over.");
            }
        }

        private void SwitchTeam(bool onNewRow)
        {
            SwitchTeamFlags flag = onNewRow ? SwitchTeamFlags.AddRow : SwitchTeamFlags.ClearRow;
            ActiveTeamIndex = ActiveTeamIndex == 0 ? 1 : 0;
            if (KnownLetters.Count(x => x == '.') > 1)
            {
                State = new WordGameStateInfo(WordGameState.SwitchTeam, flag | SwitchTeamFlags.AddBonusLetter);
            }
            else
            {
                State = new WordGameStateInfo(WordGameState.SwitchTeam, flag);
            }
        }
    }


    public enum WordGameState
    {
        NotStarted,
        Ongoing,
        SwitchTeam,
        Won,
        Lost
    }

    [Flags]
    public enum SwitchTeamFlags
    {
        Normal = 0,
        ClearRow = 1,
        AddRow = 2,
        AddBonusLetter = 4
    }

    public class WordGameStateInfo
    {
        public WordGameState State { get; }
        public SwitchTeamFlags Flags { get; }

        public WordGameStateInfo(WordGameState state)
        {
            State = state;
        }

        public WordGameStateInfo(WordGameState state, SwitchTeamFlags flags)
        {
            State = state;
            Flags = flags;
        }
    }
}
