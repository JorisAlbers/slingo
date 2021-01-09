using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Slingo.Bingo
{
    public class BingoViewModel : ReactiveObject
    {
        private readonly int[] _alreadyFilledNumbers;
        private BingoBallViewModel[][] Matrix { get; }

        public BingoBallViewModel[] FlattendMatrix => Matrix.SelectMany(x=>x).ToArray();
        
        
        public BingoViewModel(bool evenNumbers,int[] alreadyFilledNumbers, Random random)
        {
            _alreadyFilledNumbers = alreadyFilledNumbers;
            int[] numbers = Enumerable.Range(1, 50).Where(x => evenNumbers ? ( x % 2 == 0 ) : (x % 2 != 0)).ToArray();
            Shuffle(numbers,random);
            Matrix = new BingoBallViewModel[5][];
            for (int x = 0; x < 5; x += 1)
            {
                Matrix[x] = new BingoBallViewModel[5];
                for (int y = 0; y < 5; y += 1)
                {
                    Matrix[x][y] = new BingoBallViewModel(numbers[ x * 5 + y]);
                }
            }
            
            // TODO make sure already filled numbers are not already in a row.
        }

        public async Task FillInitialBalls()
        {
            foreach (BingoBallViewModel[] bingoBallViewModels in Matrix)
            {
                foreach (BingoBallViewModel viewModel in bingoBallViewModels)
                {
                    if (_alreadyFilledNumbers.Contains(viewModel.Number))
                    {
                        viewModel.Fill();
                        await Task.Delay(100);
                    }
                }
            }
        }

        private static void Shuffle<T>(IList<T> list, Random random)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
