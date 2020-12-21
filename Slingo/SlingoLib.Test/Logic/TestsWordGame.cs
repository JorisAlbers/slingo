using System.Linq;
using NUnit.Framework;
using SlingoLib.Logic;

namespace SlingoLib.Test.Logic
{
    [TestFixture()]
    public class TestsWordGame
    {
        [Test()]
        public void Solve_CorrectWord()
        {
            WordGame wordGame = new WordGame("bomen");
            wordGame.Solve("bomen");

            char[] expectedLetters = "bomen".ToCharArray();
            LetterState[] expectedStates = new LetterState[]
            {
                LetterState.CorrectLocation,
                LetterState.CorrectLocation,
                LetterState.CorrectLocation,
                LetterState.CorrectLocation,
                LetterState.CorrectLocation,
            };

            CollectionAssert.AreEqual(expectedLetters, wordGame.Submissions[0].Select(x=>x.Letter));
            CollectionAssert.AreEqual(expectedStates, wordGame.Submissions[0].Select(x=>x.State));
        }

        [Test()]
        public void Solve_InCorrectWord()
        {
            WordGame wordGame = new WordGame("bomen");
            wordGame.Solve("clown");

            char[] expectedLetters = "clown".ToCharArray();
            LetterState[] expectedStates = new LetterState[]
            {
                LetterState.DoesNotExistInWord,
                LetterState.DoesNotExistInWord,
                LetterState.IncorrectLocation,
                LetterState.DoesNotExistInWord,
                LetterState.CorrectLocation,
            };

            CollectionAssert.AreEqual(expectedLetters, wordGame.Submissions[0].Select(x => x.Letter));
            CollectionAssert.AreEqual(expectedStates, wordGame.Submissions[0].Select(x => x.State));
        }

        [Test]
        public void Solve_WordTooLong()
        {
            WordGame wordGame = new WordGame("bomen");
            wordGame.Solve("bomenverzameling");

            char[] expectedLetters = "bomen".ToCharArray();

            CollectionAssert.AreEqual(expectedLetters, wordGame.Submissions[0].Select(x => x.Letter));
            Assert.IsTrue(wordGame.Submissions[0].All(x => x.State == LetterState.DoesNotExistInWord));
        }

        [Test]
        public void Solve_WordTooShort()
        {
            WordGame wordGame = new WordGame("bomen");
            wordGame.Solve("bome");

            char[] expectedLetters = "bome.".ToCharArray();

            CollectionAssert.AreEqual(expectedLetters, wordGame.Submissions[0].Select(x => x.Letter));
            Assert.IsTrue(wordGame.Submissions[0].All(x => x.State == LetterState.DoesNotExistInWord));
        }
    }
}