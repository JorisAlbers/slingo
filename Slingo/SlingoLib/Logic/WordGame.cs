using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlingoLib.Logic
{
    public class WordGame
    {
        private char[] _word;

        public WordGame(string word)
        {
            _word = word.ToCharArray();
            Submissions = new List<WordGameLetterEntry[]>();
        }

        public List<WordGameLetterEntry[]> Submissions { get; }


        public void Solve(string word)
        {
            char[] input = word.ToCharArray();
            
            WordGameLetterEntry[] newSubmission = new WordGameLetterEntry[_word.Length];
            for (int i = 0; i < _word.Length; i++)
            {
                if (input[i] == _word[i])
                {
                    newSubmission[i] = new WordGameLetterEntry(input[i],LetterState.CorrectLocation);
                    continue;
                }

                if (_word.Contains(input[i]))
                {
                    newSubmission[i] = new WordGameLetterEntry(input[i], LetterState.IncorrectLocation);
                    continue;
                }

                newSubmission[i] = new WordGameLetterEntry(input[i], LetterState.DoesNotExistInWord);
            }

            Submissions.Add(newSubmission);
        }
    }

    public class WordGameLetterEntry
    {
        public char Letter { get; }
        public LetterState State { get; }

        public WordGameLetterEntry(char letter, LetterState state)
        {
            Letter = letter;
            State = state;
        }
    }

    public enum LetterState
    {
        DoesNotExistInWord,
        CorrectLocation,
        IncorrectLocation
    }
}
