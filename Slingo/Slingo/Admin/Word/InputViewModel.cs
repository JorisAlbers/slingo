using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using SlingoLib;
using SlingoLib.Serialization;

namespace Slingo.Admin.Word
{
    public class InputViewModel : ReactiveObject
    {
        private readonly WordRepository _wordRepository;
        private readonly Settings _settings;
        private readonly Random _random;
        private readonly List<string> _words;
        
        public BingoAdminPanelViewModel BingoAdminPanelViewModel1 { get;  }
        public BingoAdminPanelViewModel BingoAdminPanelViewModel2 { get;  }
        [Reactive] public WordInputViewModel WordInputViewModel { get; private set; }
        [Reactive] public ReactiveObject SelectedViewModel { get; private set; }

       
        public InputViewModel(WordRepository wordRepository, Settings settings)
        {
            _wordRepository = wordRepository;
            _settings = settings;
            _words = _wordRepository.Deserialize(settings.WordSize);
            _random = new Random();

            BingoAdminPanelViewModel1 = new BingoAdminPanelViewModel(0);
            BingoAdminPanelViewModel2 = new BingoAdminPanelViewModel(1);
            
            SelectedViewModel = BingoAdminPanelViewModel1;
        }
    }
}
