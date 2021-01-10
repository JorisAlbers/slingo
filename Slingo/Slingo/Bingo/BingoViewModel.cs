using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Slingo.Bingo
{
    public class BingoViewModel : ReactiveObject
    {
        private readonly int[] _alreadyFilledNumbers;
        private BingoBallViewModel[][] Matrix { get; set; }

        public BingoBallViewModel[] FlattendMatrix => Matrix.SelectMany(x=>x).ToArray();
        
        
        public BingoViewModel(bool evenNumbers,int[] alreadyFilledNumbers, Random random)
        {
            _alreadyFilledNumbers = alreadyFilledNumbers;
            int[] numbers = Enumerable.Range(1, 50).Where(x => evenNumbers ? ( x % 2 == 0 ) : (x % 2 != 0)).ToArray();
            Matrix = SetupMatrix(random, numbers, alreadyFilledNumbers);
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

        public async Task<bool> FillBall(int number)
        {
            foreach (BingoBallViewModel viewmodel in FlattendMatrix)
            {
                if(viewmodel.Number == number)
                {
                    await viewmodel.Fill();
                }
            }

            foreach (BingoBallViewModel[] line in Lines(Matrix))
            {
                int filled = line.Count(x => x.IsFilled);
                if (filled == 5)
                {
                    return true;
                }

                if (filled == 4)
                {
                    foreach (BingoBallViewModel bingoBallViewModel in line.Where(x=>!x.IsFilled))
                    {
                        bingoBallViewModel.IsMatchPoint = true;
                    }
                }
            }
            
            return false;
        }
        
        private BingoBallViewModel[][] SetupMatrix(Random random, int[] numbers, int[] filledNumbers)
        {
            BingoBallViewModel[][] matrix;
            do
            {
                Shuffle(numbers, random);
                matrix = new BingoBallViewModel[5][];
                for (int x = 0; x < 5; x += 1)
                {
                    matrix[x] = new BingoBallViewModel[5];
                    for (int y = 0; y < 5; y += 1)
                    {
                        matrix[x][y] = new BingoBallViewModel(numbers[x * 5 + y]);
                    }
                }
            } while (Lines(matrix).Any(x=>x.Count(x=> filledNumbers.Contains(x.Number)) > 3));

            return matrix;
        }

        private IEnumerable<BingoBallViewModel[]> Lines(BingoBallViewModel[][] matrix)
        {
            // Horizontal
            for (int i = 0; i < matrix.Length; i++)
            {
                BingoBallViewModel[] line = new BingoBallViewModel[matrix.Length];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    line[j] = matrix[i][j];
                }

                yield return line;
            }

            // Vertical
            for (int i = 0; i < matrix.Length; i++)
            {
                BingoBallViewModel[] line = new BingoBallViewModel[matrix.Length];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    line[j] = matrix[j][i];
                }

                yield return line;
            }

            // Diagonal
            BingoBallViewModel[] diagonalLine = new BingoBallViewModel[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                diagonalLine[i] = matrix[i][i];
            }

            yield return diagonalLine;

            diagonalLine = new BingoBallViewModel[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                diagonalLine[i] = matrix[matrix.Length - 1 - i][matrix.Length - 1 - i];
            }

            yield return diagonalLine;
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
