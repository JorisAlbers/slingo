using System.IO;
using System.IO.Abstractions;
using Moq;
using NUnit.Framework;
using SlingoLib.Serialization;

namespace SlingoLib.Test.Serialization
{
    [TestFixture()]
    public class TestsWordRepository
    {
        [Test()]
        public void Deserialize5LetterWords_FileDoesNotExist_Throws()
        {
            var fileMock = new Mock<IFileInfo>();
            fileMock.Setup(x => x.Exists).Returns(false);
            WordRepository repo = new WordRepository(fileMock.Object);
            Assert.Throws<FileNotFoundException>(() => repo.Deserialize5LetterWords());
        }
    }
}