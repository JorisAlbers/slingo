using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;
using ReactiveUI;

namespace Slingo.Game.Score
{
    /// <summary>
    /// Interaction logic for ScoreBoardControl.xaml
    /// </summary>
    public partial class ScoreBoardControl : ReactiveUserControl<ScoreboardViewModel>
    {
        public ScoreBoardControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.Score,
                        view => view.ScoreTextBlock.Text)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.IsActiveTeam,
                        view => view.OuterBorder.BorderBrush,
                        (isActiveTeam) => isActiveTeam ? new SolidColorBrush(Color.FromRgb(251, 189, 0)) : new SolidColorBrush(Color.FromRgb(199, 201, 203)))
                    .DisposeWith(dispose);

                
                // Left or right corner stuff
                this.OneWayBind(ViewModel,
                    vm => vm.HorizontalPosition,
                    view => view.OuterBorder.CornerRadius,
                    (position) =>
                    {
                        switch (position)
                        {
                            case HorizontalAlignment.Left:
                                return new CornerRadius(200, 0, 0, 200);
                            case HorizontalAlignment.Right:
                                return new CornerRadius(0, 200, 200, 0);
                            default:
                                throw new NotImplementedException();
                        }
                    });

                this.OneWayBind(ViewModel,
                    vm => vm.HorizontalPosition,
                    view => view.InnerBorder.CornerRadius,
                    (position) =>
                    {
                        switch (position)
                        {
                            case HorizontalAlignment.Left:
                                return new CornerRadius(200, 0, 0, 200);
                            case HorizontalAlignment.Right:
                                return new CornerRadius(0, 200, 200, 0);
                            default:
                                throw new NotImplementedException();
                        }
                    });

                this.OneWayBind(ViewModel,
                    vm => vm.HorizontalPosition,
                    view => view.Column1.Width,
                    (position) =>
                    {
                        switch (position)
                        {
                            case HorizontalAlignment.Left:
                                return new GridLength(10, GridUnitType.Star);
                            case HorizontalAlignment.Right:
                                return new GridLength(0);
                            default:
                                throw new NotImplementedException();
                        }
                    });

                this.OneWayBind(ViewModel,
                    vm => vm.HorizontalPosition,
                    view => view.Column2.Width,
                    (position) =>
                    {
                        switch (position)
                        {
                            case HorizontalAlignment.Right:
                                return new GridLength(10, GridUnitType.Star);
                            case HorizontalAlignment.Left:
                                return new GridLength(0);
                            default:
                                throw new NotImplementedException();
                        }
                    });
            });
        }
    }
}
