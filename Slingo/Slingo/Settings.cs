using System;
using NAudio.Wave;
using Slingo.Admin.Setup;

namespace Slingo
{
    public class Settings
    {
        public int StartingTeamIndex { get; }
        
        public int WordSize { get; }
        
        /// <summary> Seconds available to come up with the next word entry. </summary>
        public int Timeout { get; }

        /// <summary>
        /// The number of times a SLINGO needs to be achieved before the game ends.
        /// </summary>
        public int Rounds { get; }

        public int[] ExcludedBallNumbersEven { get; }
        public int[] ExcludedBallNumbersOdd { get; }
        public WaveOut AudioDevice { get; }
        public ObsSettings ObsSettings { get; }


        public Settings(int startingTeamIndex, int wordSize, int timeout, int rounds, int[] excludedBallNumbersEven,
            int[] excludedBallNumbersOdd, WaveOut audioDevice, ObsSettings obsSettings)
        {
            if (startingTeamIndex != 0 && startingTeamIndex != 1)
            {
                throw new ArgumentException("The starting team must be either 0 or 1");
            }


            WordSize = wordSize;
            Timeout = timeout;
            Rounds = rounds;
            ExcludedBallNumbersEven = excludedBallNumbersEven;
            ExcludedBallNumbersOdd = excludedBallNumbersOdd;
            AudioDevice = audioDevice;
            ObsSettings = obsSettings;
            StartingTeamIndex = startingTeamIndex;
        }
    }
}
