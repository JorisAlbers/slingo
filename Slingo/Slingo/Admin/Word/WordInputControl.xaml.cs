﻿using System;
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
using SlingoLib.Logic.Word;

namespace Slingo.Admin.Word
{
    /// <summary>
    /// Interaction logic for WordInputControl.xaml
    /// </summary>
    public partial class WordInputControl : ReactiveUserControl<WordInputViewModel>
    {
        public WordInputControl()
        {
            InitializeComponent();

            this.WhenActivated((dispose) =>
            {
                this.Bind(ViewModel,
                        vm => vm.WordInputtedByUser,
                        view => view.WordTextBox.Text)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.StateInfo.State,
                        view => view.WordTextBox.IsEnabled,
                        s=> s == WordGameState.Ongoing || s == WordGameState.SwitchTeam)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.Accept,
                        view => view.AcceptButton)
                    .DisposeWith(dispose);

                this.WordTextBox.InputBindings.Add(new KeyBinding(ViewModel.Accept, Key.Return, ModifierKeys.None));
                
                this.BindCommand(ViewModel,
                        vm => vm.Reject,
                        view => view.RejectButton)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.CandidateWord,
                        view => view.NextWordTextBlock.Text)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.GenerateWord,
                        view => view.GenerateNewWordButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.NewGame,
                        view => view.StartNewGameButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.AddRow,
                        view => view.AddRowAndSwitchTeamButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.AddBonusLetter,
                        view => view.AddBonusLetterButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.ClearRow,
                        view => view.ClearRowAndSwitchTeamButton)
                    .DisposeWith(dispose);

                this.OneWayBind(ViewModel,
                        vm => vm.CurrentWord,
                        view => view.CurrentWordTextBlock.Text)
                    .DisposeWith(dispose);

                this.Hide.Checked += (sender, args) => CurrentWordTextBlock.Visibility = Visibility.Hidden;
                this.Hide.Unchecked += (sender, args) => CurrentWordTextBlock.Visibility = Visibility.Visible;

                this.OneWayBind(ViewModel,
                        vm => vm.TimeLeftBeforeTimeOut,
                        view => view.TimeLeftTextBlock.Text)
                    .DisposeWith(dispose);

                this.Bind(ViewModel,
                        vm => vm.AutoTimeOut,
                        view => view.AutoTimeOutCheckBox.IsChecked)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.TimeOut,
                        view => view.ForceTimeOutButton)
                    .DisposeWith(dispose);

                this.BindCommand(ViewModel,
                        vm => vm.ShowWord,
                        view => view.ShowWordButton)
                    .DisposeWith(dispose);
            });
        }
    }
}
