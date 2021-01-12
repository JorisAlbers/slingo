using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace Slingo.Game.Bingo
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
                    break;
                }
            }

            foreach (BingoBallViewModel[] line in Lines(Matrix))
            {
                int filled = line.Count(x => x.State == BallState.Filled);
                if (filled == 5)
                {
                    await ShowWin(line);
                    return true;
                }

                if (filled == 4)
                {
                    foreach (BingoBallViewModel bingoBallViewModel in line.Where(x=>x.State != BallState.Filled))
                    {
                        bingoBallViewModel.IsMatchPoint = true;
                    }
                }
            }
            
            return false;
        }

        private async Task ShowWin(BingoBallViewModel[] line)
        {
            line[0].SetWinState("SL");
            line[1].SetWinState("I");
            line[2].SetWinState("N");
            line[3].SetWinState("G");
            await line[4].SetWinState("O");
            await Task.Delay(150);
            foreach (BingoBallViewModel ball in line)
            {
                ball.Flash();
                await Task.Delay(150);
            }

            await Task.Delay(100);

            for (int i = 0; i < line.Length; i++)
            {
                line[i].Flash();
                await Task.Delay(75);
            }
            
            await Task.Delay(100);

            for (int i = 0; i < line.Length; i++)
            {
                line[line.Length -1 - i].Flash();
                await Task.Delay(75);
            }

            await Task.Delay(100);
            
            for (int i = 0; i < line.Length; i++)
            {
                line[i].Flash();
                await Task.Delay(75);
            }
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
                diagonalLine[i] = matrix[matrix.Length - 1 - i][i];
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
