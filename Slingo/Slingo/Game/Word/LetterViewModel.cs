using ReactiveUI;
using SlingoLib.Logic.Word;

namespace Slingo.Game.Word
{
    public class LetterViewModel: ReactiveObject
    {
        public char Letter { get; }
        public LetterState LetterState { get; }

        public LetterViewModel(char letter, LetterState letterState)
        {
            Letter = char.ToUpper(letter);
            LetterState = letterState;
        }
    }
}
