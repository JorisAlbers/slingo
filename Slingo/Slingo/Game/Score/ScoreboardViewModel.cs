using System;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Word;

namespace Slingo.Game.Score
{
    public class ScoreboardViewModel : ReactiveObject
    {
        private readonly TeamState _state;
        [Reactive] public int Score { get; private set; }
        [Reactive] public bool IsActiveTeam { get; private set; }
        public HorizontalAlignment HorizontalPosition { get;}

        public ScoreboardViewModel(TeamState state, HorizontalAlignment horizontalPosition)
        {
            _state = state;
            HorizontalPosition = horizontalPosition;

            this.WhenAnyValue(x => x._state.Score).Subscribe(x => Score = x);
            this.WhenAnyValue(x => x._state.IsActiveTeam).Subscribe(x => IsActiveTeam = x);
        }
    }
}
