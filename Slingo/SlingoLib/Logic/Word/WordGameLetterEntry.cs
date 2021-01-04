namespace SlingoLib.Logic.Word
{
    public class WordGameLetterEntry
    {
        public char Letter { get; }
        public LetterState State { get; }

        public WordGameLetterEntry(char letter, LetterState state)
        {
            Letter = letter;
            State = state;
        }
    }
}