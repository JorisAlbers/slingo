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
        }

        public WordGameEntry Solve(string inputWord)
        {
            char[] inputCharArray = inputWord.ToCharArray();
            if (inputCharArray.Length != _word.Length)
            {
               return new WordGameEntry(CreateIncorrectLengthSubmission(inputCharArray));
            }

            
            WordGameLetterEntry[] newSubmission = new WordGameLetterEntry[_word.Length];
            // first check for the correct locations
            for (int i = 0; i < _word.Length; i++)
            {
                if (inputCharArray[i] == _word[i])
                {
                    newSubmission[i] = new WordGameLetterEntry(inputCharArray[i],LetterState.CorrectLocation);
                }
            }
            // Then check for incorrect locations
            for (int i = 0; i < _word.Length; i++)
            {
                if (newSubmission[i] != null)
                {
                    continue;
                }
                
                int timesInputCharExistInWord = _word.Count(x=>x == inputCharArray[i]);
                if (timesInputCharExistInWord > 0)
                {
                    int timesAlreadyIndicated = newSubmission.Count(x => x!= null && x.Letter == inputCharArray[i]);
                    if (timesInputCharExistInWord > timesAlreadyIndicated)
                    {
                        newSubmission[i] = new WordGameLetterEntry(inputCharArray[i], LetterState.IncorrectLocation);
                        continue;
                    }
                }
                
                newSubmission[i] = new WordGameLetterEntry(inputCharArray[i], LetterState.DoesNotExistInWord);
            }



            return new WordGameEntry(newSubmission);
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
}
