using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlingoLib.Logic.Word
{
    public class WordGame
    {
        private readonly WordPuzzle _wordPuzzle;

        public WordGame(WordPuzzle wordPuzzle, int startingTeamIndex)
        {
            _wordPuzzle = wordPuzzle;
            KnownLetters = wordPuzzle.Word[0] + new string('.', wordPuzzle.Word.Length - 1);
            ActiveTeamIndex = startingTeamIndex;
            
        }
        
        public string KnownLetters { get; private set; }
        
        public int ActiveTeamIndex { get; private set; }
        
        public int AttemptIndex { get; private set; }
        
        public WordGameState State { get; private set; }

        public WordPuzzleEntry Solve(string word)
        {
            if (State == WordGameState.Won || State == WordGameState.Lost)
            {
                throw new InvalidOperationException("Game is already over.");
            }
            AttemptIndex++;
            
            var puzzleEntry = _wordPuzzle.Solve(word);
            if (puzzleEntry.LetterEntries.All(x => x.State == LetterState.CorrectLocation))
            {
                State = WordGameState.Won;
                return puzzleEntry;
            }
            
            UpdateKnownLetters(puzzleEntry);
            
            if (AttemptIndex == 6)
            {
                State = WordGameState.Lost;
            }
            else if(AttemptIndex == 5)
            {
                ActiveTeamIndex = ActiveTeamIndex == 0 ? 1 : 0;
                State = WordGameState.SwitchTeam;
            }
            else
            {
                State = WordGameState.Ongoing;
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
            if (State == WordGameState.Won || State == WordGameState.Lost)
            {
                throw new InvalidOperationException("Game is already over.");
            }

            AttemptIndex++;

            if (AttemptIndex == 6)
            {
                State = WordGameState.Lost;
            }
            else if (AttemptIndex == 5)
            {
                ActiveTeamIndex = ActiveTeamIndex == 0 ? 1 : 0;
                State = WordGameState.SwitchTeam;
            }
            else
            {
                State = WordGameState.Ongoing;
            }
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
    }
    

    public enum WordGameState
    {
        NotStarted,
        Ongoing,
        SwitchTeam,
        Won,
        Lost
    }

    
    
}
