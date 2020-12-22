using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace SlingoLib.Serialization
{
    public class WordRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _filePath;
        private const string _ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyzĳ";
        private readonly char[] _allowedCharsAsArray = _ALLOWED_CHARS.ToCharArray();

        public WordRepository(IFileSystem fileSystem, string filePath)
        {
            _fileSystem = fileSystem;
            _filePath = filePath;
        }

        public List<string> Deserialize(int wordLength)
        {
            if (!_fileSystem.File.Exists(_filePath))
            {
                throw new FileNotFoundException();
            }

            List<string> words = new List<string>();
            using (StreamReader reader = new StreamReader(_fileSystem.File.OpenRead(_filePath)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string word = line.Replace("\n", "").Replace("\r", "");
                    word = word.Replace("ij", "ĳ"); // digraph 'ij' to monograph ij

                    if (WordIsAllowed(wordLength,word))
                    {
                        words.Add(word);
                    }
                }
            }

            return words;
        }

        private bool WordIsAllowed(int wordLength,string line)
        {
            if (line.Length != wordLength)
            {
                return false;
            }

            foreach (char letter in line)
            {
                if (!_allowedCharsAsArray.Contains(letter))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
