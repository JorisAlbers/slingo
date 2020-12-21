using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace SlingoLib.Serialization
{
    public class WordRepository
    {
        private readonly IFileInfo _fiveLetterWords;

        public WordRepository(IFileInfo fiveLetterWords)
        {
            _fiveLetterWords = fiveLetterWords;
        }

        public List<string> Deserialize5LetterWords()
        {
            if (!_fiveLetterWords.Exists)
            {
                throw new FileNotFoundException();
            }

            return null;
        }
    }
}
