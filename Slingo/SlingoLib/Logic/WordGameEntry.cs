namespace SlingoLib.Logic
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