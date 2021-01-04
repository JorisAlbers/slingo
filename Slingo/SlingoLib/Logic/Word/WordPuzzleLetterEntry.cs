namespace SlingoLib.Logic.Word
{
    public class WordPuzzleLetterEntry
    {
        public char Letter { get; }
        public LetterState State { get; }

        public WordPuzzleLetterEntry(char letter, LetterState state)
        {
            Letter = letter;
            State = state;
        }
    }
}