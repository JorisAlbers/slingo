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


        public void Solve(string inputWord)
        {
            char[] inputCharArray = inputWord.ToCharArray();
            if (inputCharArray.Length != _word.Length)
            {
                Submissions.Add(CreateIncorrectLengthSubmission(inputCharArray));
                return;
            }
            
            WordGameLetterEntry[] newSubmission = new WordGameLetterEntry[_word.Length];
            for (int i = 0; i < _word.Length; i++)
            {
                if (inputCharArray[i] == _word[i])
                {
                    newSubmission[i] = new WordGameLetterEntry(inputCharArray[i],LetterState.CorrectLocation);
                    continue;
                }

                if (_word.Contains(inputCharArray[i]))
                {
                    newSubmission[i] = new WordGameLetterEntry(inputCharArray[i], LetterState.IncorrectLocation);
                    continue;
                }

                newSubmission[i] = new WordGameLetterEntry(inputCharArray[i], LetterState.DoesNotExistInWord);
            }

            Submissions.Add(newSubmission);
        }

        private WordGameLetterEntry[] CreateIncorrectLengthSubmission(char[] input)
        {
            WordGameLetterEntry[] newSubmission = new WordGameLetterEntry[_word.Length];
            for (int i = 0; i < _word.Length; i++)
            {
                if (input.Length -1 < i)
                {
                    newSubmission[i] = new WordGameLetterEntry('.',LetterState.DoesNotExistInWord);
                }
                else
                {
                    newSubmission[i] = new WordGameLetterEntry(input[i],LetterState.DoesNotExistInWord);
                }
            }

            return newSubmission;
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
