using ReactiveUI;
using Slingo.Sound;
using SlingoLib;

namespace Slingo.WordGame
{
    public class WordGameViewModel : ReactiveObject
    {
        private readonly Settings _settings;
        
        public BoardViewModel BoardViewModel { get; }

        public WordGameViewModel(Settings settings, AudioPlaybackEngine audioPlaybackEngine)
        {
            _settings = settings;
            // todo contain gameLogic
            BoardViewModel = new BoardViewModel(settings.WordSize, audioPlaybackEngine);
        }
    }
}