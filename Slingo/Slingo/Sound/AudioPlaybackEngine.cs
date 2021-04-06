using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Slingo.Sound
{
    public class AudioPlaybackEngine : IDisposable
    {
        private readonly WaveOut _waveOut;
        private readonly MixingSampleProvider _mixer;

        public AudioPlaybackEngine(WaveOut waveOut, int sampleRate = 44100, int channelCount = 2)
        {
            _waveOut = waveOut;
            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            _mixer.ReadFully = true;
            _waveOut.Init(_mixer);
            _waveOut.Play();
        }
        
        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == _mixer.WaveFormat.Channels)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && _mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(input);
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        public void PlaySound(CachedSound sound)
        {
            AddMixerInput(new CachedSoundSampleProvider(sound));
        }
        
        public void PlaySound(ISampleProvider provider)
        {
            AddMixerInput(provider);
        }

        private void AddMixerInput(ISampleProvider input)
        {
            _mixer.AddMixerInput(ConvertToRightChannelCount(input));
        }

        public void Dispose()
        {
            _waveOut.Dispose();
        }
    }
}
