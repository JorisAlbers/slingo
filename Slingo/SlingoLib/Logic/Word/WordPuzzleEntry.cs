namespace SlingoLib.Logic.Word
{
    public class WordPuzzleEntry
    {
        public WordPuzzleLetterEntry[] LetterEntries { get; }

        public WordPuzzleEntry(WordPuzzleLetterEntry[] letterEntries)
        {
            LetterEntries = letterEntries;
        }
    }
}