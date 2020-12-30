using System;
using System.Linq;
using NAudio.Wave;

namespace Slingo.Sound
{
    public class CombinedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound[] _cachedSounds;
        private readonly int _millisecondsPerSound;
        private long _position;
        private int _soundIndex;

        public CombinedSoundSampleProvider(CachedSound[] cachedSounds, int millisecondsPerSound)
        {
            if (!cachedSounds.Skip(1).All(x => x.WaveFormat.Equals(cachedSounds[0].WaveFormat)))
            {
                throw new InvalidOperationException("The Waveformats must be of the same type");
            }
            
            _cachedSounds = cachedSounds;
            _millisecondsPerSound = millisecondsPerSound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (_soundIndex > _cachedSounds.Length -1)
            {
                return 0;
            }

            long positionThisBuffer = 0;
            CachedSound currentSound = _cachedSounds[_soundIndex];
            
            // Copy sound
            var availableSamples = Math.Max(currentSound.AudioData.Length - _position,0);
            var samplesToCopy = Math.Min(availableSamples, count);
            if (availableSamples > 0)
            {
                Array.Copy(currentSound.AudioData, _position, buffer, offset, samplesToCopy);
                _position += samplesToCopy;
                positionThisBuffer += samplesToCopy;
            }

            // If the sound ended
            if (availableSamples < count)
            {
                int soundDuration = MillisecondsInSound(currentSound);
                int breakDuration = Math.Max(_millisecondsPerSound - soundDuration, 0);
                int samplesInBreak = SamplesPerMillisecond(currentSound) * breakDuration;
                long samplesInBreakPassed = _position - currentSound.AudioData.Length;
                long samplesInBreakLeft = samplesInBreak - samplesInBreakPassed;
                long breakLengthThisBuffer = 0;
                if (samplesInBreakLeft > 0)
                {
                    breakLengthThisBuffer = Math.Min(samplesInBreakLeft, count - samplesToCopy);
                    Array.Clear(buffer,(int) (offset + positionThisBuffer), (int) breakLengthThisBuffer);
                    _position += breakLengthThisBuffer;
                    positionThisBuffer += breakLengthThisBuffer;
                }

                if (breakLengthThisBuffer == samplesInBreakLeft)
                {
                    // go to next sample
                    _soundIndex++;
                    _position = 0;
                    // add bytes from the next sample
                    positionThisBuffer += Read(buffer, (int)(offset + positionThisBuffer), (int)(count - positionThisBuffer));
                }
            }

            return (int) positionThisBuffer;
        }
        
        private int MillisecondsInSound(CachedSound sound)
        {
            int sampleRatePerMillisecond = sound.WaveFormat.SampleRate / 1000;
            float milliseconds = (sound.AudioData.Length / sampleRatePerMillisecond) / sound.WaveFormat.Channels;
            return (int) Math.Round(milliseconds);
        }

        private int SamplesPerMillisecond(CachedSound sound)
        {
            int sampleRatePerMillisecond = sound.WaveFormat.SampleRate / 1000;
            return sampleRatePerMillisecond * sound.WaveFormat.Channels;
        }

        public WaveFormat WaveFormat => _cachedSounds[0].WaveFormat;
    }
}