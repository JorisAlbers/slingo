using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin;
using Splat;

namespace Slingo.Game
{
    public class GameWindowViewModel : ReactiveObject
    {
        private AdminViewModel _adminViewModel;
        private AdminWindow _adminWindow;

        [Reactive] public ReactiveObject SelectedViewModel { get; set; }
        
        public GameWindowViewModel()
        {
            _adminViewModel = new AdminViewModel();
            var view = Locator.Current.GetService<IViewFor<AdminViewModel>>();
            var window = view as ReactiveWindow<AdminViewModel>;
            window.ViewModel = _adminViewModel;
            window.Show();
        }
        
    }
}
