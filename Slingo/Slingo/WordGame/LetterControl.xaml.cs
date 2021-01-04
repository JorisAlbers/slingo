using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;
using SlingoLib.Logic;
using SlingoLib.Logic.Word;

namespace Slingo.WordGame
{
    /// <summary>
    /// Interaction logic for LetterControl.xaml
    /// </summary>
    public partial class LetterControl : ReactiveUserControl<LetterViewModel>
    {
        public LetterControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                    vm => vm.Letter,
                    view => view.LetterTextBlock.Text);

                this.OneWayBind(ViewModel,
                    vm => vm.LetterState,
                    view => view.OuterBorder.Background,
                    (state) =>
                    {
                        switch (state)
                        {
                            case LetterState.IncorrectLocation:
                            case LetterState.DoesNotExistInWord:
                                return new SolidColorBrush(Color.FromRgb(3, 67, 96));
                            case LetterState.CorrectLocation:
                                return new SolidColorBrush(Color.FromRgb(184, 66, 50));
                            default:
                                throw new ArgumentOutOfRangeException(nameof(state), state, null);
                        }
                    });

                this.OneWayBind(ViewModel,
                    vm => vm.LetterState,
                    view => view.InnerBorder.Background,
                    (state) =>
                    {
                        switch (state)
                        {
                            case LetterState.DoesNotExistInWord:
                            case LetterState.CorrectLocation:
                                return new SolidColorBrush(Colors.Transparent);
                            case LetterState.IncorrectLocation:
                                return new SolidColorBrush(Color.FromRgb(214, 188, 25));
                            default:
                                throw new ArgumentOutOfRangeException(nameof(state), state, null);
                        }
                    });
            });
        }
    }
}
