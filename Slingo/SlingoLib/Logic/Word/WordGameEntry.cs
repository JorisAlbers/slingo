namespace SlingoLib.Logic.Word
{
    public class WordGameEntry
    {
        public WordGameLetterEntry[] LetterEntries { get; }

        public WordGameEntry(WordGameLetterEntry[] letterEntries)
        {
            LetterEntries = letterEntries;
        }
    }
}