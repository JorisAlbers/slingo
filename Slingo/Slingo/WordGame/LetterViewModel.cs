using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using SlingoLib.Logic;
using SlingoLib.Logic.Word;

namespace Slingo.WordGame
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
