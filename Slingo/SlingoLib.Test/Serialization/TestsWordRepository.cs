using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Moq;
using NUnit.Framework;
using SlingoLib.Serialization;

namespace SlingoLib.Test.Serialization
{
    [TestFixture()]
    public class TestsWordRepository
    {
        [Test()]
        public void Deserialize_FileDoesNotExist_Throws()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            WordRepository repo = new WordRepository(fileSystem.FileSystem, "words.txt");
            Assert.Throws<FileNotFoundException>(() => repo.Deserialize(5));
        }

        [TestCase("thisWordIsTooLong")]
        [TestCase("abcd5")]
        [TestCase("Abcdf")]
        [TestCase("ab df")]
        [TestCase("ab-df")]
        [TestCase("ab,df")]
        [TestCase("abédf")]
        [TestCase("ab|df")]
        public void Deserialize_IgnoresWordsWhichAreNotAllowed(string word)
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\words.txt", new MockFileData(word) },
            });
            
            WordRepository repo = new WordRepository(fileSystem.FileSystem, @"c:\words.txt");
            Assert.IsEmpty(repo.Deserialize(5));
        }

        [TestCase("bomen")]
        [TestCase("zuren")]
        [TestCase("clown")]
        public void Deserialize_ReturnsAllowedWord(string word)
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\words.txt", new MockFileData(word) },
            });

            WordRepository repo = new WordRepository(fileSystem.FileSystem, @"c:\words.txt");
            var result = repo.Deserialize(5);
            CollectionAssert.AreEqual(new string[] { word }, result);
        }

        [Test]
        public void Deserialize_IJIsSeenAsSingleLetter()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\words.txt", new MockFileData("ijsjes") },
            });

            WordRepository repo = new WordRepository(fileSystem.FileSystem, @"c:\words.txt");
            var result = repo.Deserialize(5);
            CollectionAssert.AreEqual(new string[]{ "ĳsjes" }, result);
            
        }
    }
}