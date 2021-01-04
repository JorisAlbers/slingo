using System.Linq;
using NUnit.Framework;
using SlingoLib.Logic.Word;

namespace SlingoLib.Test.Logic.Word
{
    [TestFixture()]
    public class TestsWordPuzzle
    {
        [Test()]
        public void Solve_CorrectWord()
        {
            WordPuzzle wordPuzzle = new WordPuzzle("bomen");
            var result = wordPuzzle.Solve("bomen");

            char[] expectedLetters = "bomen".ToCharArray();
            LetterState[] expectedStates = new LetterState[]
            {
                LetterState.CorrectLocation,
                LetterState.CorrectLocation,
                LetterState.CorrectLocation,
                LetterState.CorrectLocation,
                LetterState.CorrectLocation,
            };

            CollectionAssert.AreEqual(expectedLetters, result.LetterEntries.Select(x=>x.Letter));
            CollectionAssert.AreEqual(expectedStates, result.LetterEntries.Select(x=>x.State));
        }

        [Test()]
        public void Solve_InCorrectWord()
        {
            WordPuzzle wordPuzzle = new WordPuzzle("bomen");
            var result = wordPuzzle.Solve("clown");

            char[] expectedLetters = "clown".ToCharArray();
            LetterState[] expectedStates = new LetterState[]
            {
                LetterState.DoesNotExistInWord,
                LetterState.DoesNotExistInWord,
                LetterState.IncorrectLocation,
                LetterState.DoesNotExistInWord,
                LetterState.CorrectLocation,
            };

            CollectionAssert.AreEqual(expectedLetters, result.LetterEntries.Select(x => x.Letter));
            CollectionAssert.AreEqual(expectedStates, result.LetterEntries.Select(x => x.State));
        }

        [Test()]
        public void Solve_LetterInIncorrectLocationIsOnlySetToIncorrectLocationNTimesItExistsInTheWord()
        {
            WordPuzzle wordPuzzle = new WordPuzzle("yxxyy");
            var result = wordPuzzle.Solve("zzxxx");

            char[] expectedLetters = "zzxxx".ToCharArray();
            LetterState[] expectedStates = new LetterState[]
            {
                LetterState.DoesNotExistInWord,
                LetterState.DoesNotExistInWord,
                LetterState.CorrectLocation, 
                LetterState.IncorrectLocation,
                LetterState.DoesNotExistInWord,
            };

            CollectionAssert.AreEqual(expectedLetters, result.LetterEntries.Select(x => x.Letter));
            CollectionAssert.AreEqual(expectedStates, result.LetterEntries.Select(x => x.State));
        }

        [Test]
        public void Solve_WordTooLong()
        {
            WordPuzzle wordPuzzle = new WordPuzzle("bomen");
            var result = wordPuzzle.Solve("bomenverzameling");

            char[] expectedLetters = "bomen".ToCharArray();

            CollectionAssert.AreEqual(expectedLetters, result.LetterEntries.Select(x => x.Letter));
            Assert.IsTrue(result.LetterEntries.All(x => x.State == LetterState.DoesNotExistInWord));
        }

        [Test]
        public void Solve_WordTooShort()
        {
            WordPuzzle wordPuzzle = new WordPuzzle("bomen");
            var result = wordPuzzle.Solve("bome");

            char[] expectedLetters = "bome.".ToCharArray();

            CollectionAssert.AreEqual(expectedLetters, result.LetterEntries.Select(x => x.Letter));
            Assert.IsTrue(result.LetterEntries.All(x => x.State == LetterState.DoesNotExistInWord));
        }
    }
}