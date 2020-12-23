using ReactiveUI;
using SlingoLib;

namespace Slingo.WordGame
{
    public class WordGameViewModel : ReactiveObject
    {
        private readonly Settings _settings;
        
        public BoardViewModel BoardViewModel { get; }

        public WordGameViewModel(Settings settings)
        {
            _settings = settings;
            BoardViewModel = new BoardViewModel(settings.WordSize);
        }
    }
}