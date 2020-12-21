﻿using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace SlingoLib.Serialization
{
    public class WordRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _filePath;
        private const string _ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyz";
        private readonly char[] _allowedCharsAsArray = _ALLOWED_CHARS.ToCharArray();

        public WordRepository(IFileSystem fileSystem, string filePath)
        {
            _fileSystem = fileSystem;
            _filePath = filePath;
        }

        public List<string> Deserialize5LetterWords()
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
                    string word = line.Substring(0, line.Length - 2); // remove \r\n
                    if (WordIsAllowed(word))
                    {
                        words.Add(word);
                    }
                }
            }

            return words;
        }

        private bool WordIsAllowed(string line)
        {
            if (line.Length != 5)
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