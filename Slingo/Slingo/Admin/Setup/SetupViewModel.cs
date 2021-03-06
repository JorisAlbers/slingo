﻿using System.Linq;
using System.Reactive;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SlingoLib;

namespace Slingo.Admin.Setup
{
    public class SetupViewModel : ReactiveObject
    {
        private readonly int[] _excludedBallNumbersEven = new int[] {6,10,12,26,34,36,42,46};
        private readonly int[] _excludedBallNumbersOdd = new int[] {1,3,11,19,27,29,43,47};
       
        [Reactive] public int WordSize { get; set; } = 5;
        [Reactive] public int TimeOut { get; set; } = 8;
        [Reactive] public int Rounds { get; set; } = 3;

        [Reactive] public bool Team1Starts { get; set; } = true;
        [Reactive] public bool Team2Starts { get; set; }
        
        public string[] AudioOutputDevices { get; }
        
        [Reactive] public string SelectedAudioOutput { get; set; }
        
        public ReactiveCommand<Unit,Settings> Start { get; }

        public SetupViewModel()
        {
            using (var devices = new MMDeviceEnumerator())
            {
                AudioOutputDevices = devices.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                    .Select(x => x.FriendlyName).ToArray();

                SelectedAudioOutput = AudioOutputDevices.First();
            }
            
            Start = ReactiveCommand.Create(() =>
            {
                return new Settings(Team1Starts? 0 : 1,WordSize,TimeOut,Rounds, _excludedBallNumbersEven, _excludedBallNumbersOdd, GetAudioDevice(SelectedAudioOutput));
            });
        }

        private WaveOut GetAudioDevice(string selectedAudioOutput)
        {
            int i;
            
            for (i = WaveOut.DeviceCount -1; i-->0;)
            {
                var device = WaveOut.GetCapabilities(i);
                if (selectedAudioOutput.StartsWith(device.ProductName))
                {
                    break;
                }
            }

            var w = new WaveOut();
            w.DeviceNumber = i;
            return w;
        }
    }
}