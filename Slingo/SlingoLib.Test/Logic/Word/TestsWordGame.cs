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
            Assert.AreEqual(WordGameState.NotStarted, wordGame.State.State);
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
            Assert.AreEqual(1, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.Won, wordGame.State.State);
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
            Assert.AreEqual(WordGameState.Ongoing, wordGame.State.State);
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
            Assert.AreEqual(WordGameState.SwitchTeam, wordGame.State.State);
            Assert.IsTrue((wordGame.State.Flags & SwitchTeamFlags.AddRow) == SwitchTeamFlags.AddRow);
            Assert.IsTrue((wordGame.State.Flags & SwitchTeamFlags.AddBonusLetter) == SwitchTeamFlags.AddBonusLetter);
        }

        [Test()]
        public void Solve_6IncorrectAnswers_GameIsLostAndOtherTeamIsStillActive()
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
            wordGame.Solve("aaaaa");

            Assert.AreEqual(1, wordGame.ActiveTeamIndex);
            Assert.AreEqual(6, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.Lost, wordGame.State.State);
        }

        [Test()]
        public void Reject_KeepsIndexButSwitchesTeam()
        {
            var puzzleEntry = new WordPuzzleEntry(Enumerable.Repeat(new WordPuzzleLetterEntry('a', LetterState.IncorrectLocation), 5).ToArray());


            var wordPuzzleMock = new Mock<WordPuzzle>("aaaa");
            wordPuzzleMock.Setup(x => x.Solve(It.IsAny<string>())).Returns(puzzleEntry);


            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            wordGame.Reject();

            Assert.AreEqual(1, wordGame.ActiveTeamIndex);
            Assert.AreEqual(0, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.SwitchTeam, wordGame.State.State);
            Assert.IsTrue((wordGame.State.Flags & SwitchTeamFlags.ClearRow) == SwitchTeamFlags.ClearRow);
            Assert.IsTrue((wordGame.State.Flags & SwitchTeamFlags.AddBonusLetter) == SwitchTeamFlags.AddBonusLetter);
        }

        [Test()]
        public void Reject_WordAlmostGuessed_StateSetToSwitchTeam()
        {
            var puzzleEntry = new WordPuzzleEntry(new[]
            {
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('b', LetterState.IncorrectLocation),
            });

            var wordPuzzleMock = new Mock<WordPuzzle>("aaaaa");
            wordPuzzleMock.Setup(x => x.Solve(It.IsAny<string>())).Returns(puzzleEntry);


            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            wordGame.Solve("aaaab");
            wordGame.Reject();

            Assert.AreEqual(1, wordGame.ActiveTeamIndex);
            Assert.AreEqual(1, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.SwitchTeam, wordGame.State.State);
            Assert.IsTrue((wordGame.State.Flags & SwitchTeamFlags.ClearRow) == SwitchTeamFlags.ClearRow);
        }

        [Test()]
        public void Timeout_WordAlmostGuessed_StateSetToSwitchTeam()
        {
            var puzzleEntry = new WordPuzzleEntry(new[]
            {
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('b', LetterState.IncorrectLocation),
            });

            var wordPuzzleMock = new Mock<WordPuzzle>("aaaaa");
            wordPuzzleMock.Setup(x => x.Solve(It.IsAny<string>())).Returns(puzzleEntry);


            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            wordGame.Solve("aaaab");
            wordGame.TimeOut();

            Assert.AreEqual(1, wordGame.ActiveTeamIndex);
            Assert.AreEqual(1, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.SwitchTeam, wordGame.State.State);
            Assert.AreEqual(SwitchTeamFlags.ClearRow,wordGame.State.Flags);
        }

        [Test()]
        public void Timeout_NoCorrectGuess_StateSetToSwitchTeamAndAddBonusLetter()
        {
            var puzzleEntry = new WordPuzzleEntry(new[]
            {
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('a', LetterState.CorrectLocation),
                new WordPuzzleLetterEntry('b', LetterState.IncorrectLocation),
                new WordPuzzleLetterEntry('b', LetterState.IncorrectLocation),
            });

            var wordPuzzleMock = new Mock<WordPuzzle>("aaaaa");
            wordPuzzleMock.Setup(x => x.Solve(It.IsAny<string>())).Returns(puzzleEntry);


            WordGame wordGame = new WordGame(wordPuzzleMock.Object, 0);
            wordGame.Solve("aaabb");
            wordGame.TimeOut();

            Assert.AreEqual(1, wordGame.ActiveTeamIndex);
            Assert.AreEqual(1, wordGame.AttemptIndex);
            Assert.AreEqual(WordGameState.SwitchTeam, wordGame.State.State);
            Assert.IsTrue((wordGame.State.Flags & SwitchTeamFlags.AddRow) != SwitchTeamFlags.AddRow);
            Assert.IsTrue((wordGame.State.Flags & SwitchTeamFlags.AddBonusLetter) == SwitchTeamFlags.AddBonusLetter);
        }
    }
}