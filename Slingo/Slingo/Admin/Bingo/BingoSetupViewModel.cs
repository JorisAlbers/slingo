using System.Reactive;
using ReactiveUI;

namespace Slingo.Admin.Bingo
{
    public class BingoSetupViewModel : ReactiveObject
    {
        private readonly BingoCardSettings _settings;
        public int TeamIndex { get; }

        public ReactiveCommand<Unit, BingoCardSettings> Initialize { get;}

        public BingoSetupViewModel(int teamIndex, BingoCardSettings settings)
        {
            _settings = settings;
            TeamIndex = teamIndex + 1;

            Initialize = ReactiveCommand.Create(() => _settings);
        }
    }

    public class BingoCardSettings
    {
        public bool EvenNumber { get; }
        public int[] FilledBalls { get; }

        public BingoCardSettings(bool evenNumber, int[] filledBalls)
        {
            EvenNumber = evenNumber;
            FilledBalls = filledBalls;
        }
    }
}