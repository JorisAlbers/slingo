using System.Linq;
using Moq;
using NUnit.Framework;
using SlingoLib.Logic.Word;

namespace SlingoLib.Test.Logic.Word
{
    [TestFixture()]
    public class TestsWordGame
    {

        [Test()]
        public void Ctor_IntializedCorrectly()
        {
            var wordPuzzleMock = new Mock<WordPuzzle>("aaaa");
            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            Assert.AreEqual(0, wordGame.ActiveTeamIndex);
            Assert.AreEqual(0, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.Ongoing, wordGame.State);
        }

        [Test()]
        public void Solve_CorrectAnswer_StateIsSetToWon()
        {
            var puzzleEntry = new WordPuzzleEntry(Enumerable.Repeat(new WordPuzzleLetterEntry('a', LetterState.CorrectLocation), 5).ToArray());


            var wordPuzzleMock = new Mock<WordPuzzle>("aaaaa");
            wordPuzzleMock.Setup(x => x.Solve(It.IsAny<string>())).Returns(puzzleEntry);


            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            wordGame.Solve("aaaaa");
            
            Assert.AreEqual(0, wordGame.ActiveTeamIndex);
            Assert.AreEqual(0, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.Won, wordGame.State);
        }

        [Test()]
        public void Solve_InCorrectAnswer_StateIsSetToOnGoing()
        {
            var puzzleEntry = new WordPuzzleEntry(Enumerable.Repeat(new WordPuzzleLetterEntry('a', LetterState.IncorrectLocation), 5).ToArray());


            var wordPuzzleMock = new Mock<WordPuzzle>("aaaaa");
            wordPuzzleMock.Setup(x => x.Solve(It.IsAny<string>())).Returns(puzzleEntry);


            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            wordGame.Solve("bbbbb");

            Assert.AreEqual(0, wordGame.ActiveTeamIndex);
            Assert.AreEqual(1, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.Ongoing, wordGame.State);
        }

        [Test()]
        public void Solve_5IncorrectAnswers_SwitchesTeam()
        {
            var puzzleEntry = new WordPuzzleEntry(Enumerable.Repeat(new WordPuzzleLetterEntry('a', LetterState.IncorrectLocation), 5).ToArray());


            var wordPuzzleMock = new Mock<WordPuzzle>("aaaa");
            wordPuzzleMock.Setup(x => x.Solve(It.IsAny<string>())).Returns(puzzleEntry);


            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            wordGame.Solve("aaaaa");
            wordGame.Solve("aaaaa");
            wordGame.Solve("aaaaa");
            wordGame.Solve("aaaaa");
            wordGame.Solve("aaaaa");

            Assert.AreEqual(1, wordGame.ActiveTeamIndex);
            Assert.AreEqual(5, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.SwitchTeam, wordGame.State);
        }

        [Test()]
        public void Reject_MovesToNextAttemptIndex()
        {
            var puzzleEntry = new WordPuzzleEntry(Enumerable.Repeat(new WordPuzzleLetterEntry('a', LetterState.IncorrectLocation), 5).ToArray());


            var wordPuzzleMock = new Mock<WordPuzzle>("aaaa");
            wordPuzzleMock.Setup(x => x.Solve(It.IsAny<string>())).Returns(puzzleEntry);


            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            wordGame.Reject();
            wordGame.Reject();
            wordGame.Reject();
            wordGame.Reject();
            wordGame.Reject();

            Assert.AreEqual(1, wordGame.ActiveTeamIndex);
            Assert.AreEqual(5, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.SwitchTeam, wordGame.State);
        }
    }
}