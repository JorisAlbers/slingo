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
            ActiveTeamIndex = startingTeamIndex;
        }
        
        public int ActiveTeamIndex { get; private set; }
        
        public int AttemptIndex { get; private set; }
        
        public WordGameState State { get; private set; }

        public WordPuzzleEntry Solve(string word)
        {
            if (State == WordGameState.Won)
            {
                throw new InvalidOperationException("Game is already over.");
            }

            var puzzleEntry = _wordPuzzle.Solve(word);
            if (puzzleEntry.LetterEntries.All(x => x.State == LetterState.CorrectLocation))
            {
                State = WordGameState.Won;
                return puzzleEntry;
            }
            
            
            if(AttemptIndex++ > 3)
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
    }
    

    public enum WordGameState
    {
        Ongoing,
        SwitchTeam,
        Won
    }

    
    
}
