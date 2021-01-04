using System.Linq;

namespace SlingoLib.Logic.Word
{
    public class WordPuzzle
    {
        private char[] _word;

        public WordPuzzle(string word)
        {
            _word = word.ToCharArray();
        }

        public WordPuzzleEntry Solve(string inputWord)
        {
            char[] inputCharArray = inputWord.ToCharArray();
            if (inputCharArray.Length != _word.Length)
            {
               return new WordPuzzleEntry(CreateIncorrectLengthSubmission(inputCharArray));
            }

            
            WordPuzzleLetterEntry[] newSubmission = new WordPuzzleLetterEntry[_word.Length];
            // first check for the correct locations
            for (int i = 0; i < _word.Length; i++)
            {
                if (inputCharArray[i] == _word[i])
                {
                    newSubmission[i] = new WordPuzzleLetterEntry(inputCharArray[i],LetterState.CorrectLocation);
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
                        newSubmission[i] = new WordPuzzleLetterEntry(inputCharArray[i], LetterState.IncorrectLocation);
                        continue;
                    }
                }
                
                newSubmission[i] = new WordPuzzleLetterEntry(inputCharArray[i], LetterState.DoesNotExistInWord);
            }



            return new WordPuzzleEntry(newSubmission);
        }

        private WordPuzzleLetterEntry[] CreateIncorrectLengthSubmission(char[] input)
        {
            WordPuzzleLetterEntry[] newSubmission = new WordPuzzleLetterEntry[_word.Length];
            for (int i = 0; i < _word.Length; i++)
            {
                if (input.Length -1 < i)
                {
                    newSubmission[i] = new WordPuzzleLetterEntry('.',LetterState.DoesNotExistInWord);
                }
                else
                {
                    newSubmission[i] = new WordPuzzleLetterEntry(input[i],LetterState.DoesNotExistInWord);
                }
            }

            return newSubmission;
        }
    }
}
