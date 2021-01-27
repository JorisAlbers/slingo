using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Slingo.Admin.Bingo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slingo.Sound;

namespace Slingo.Game.Bingo
{
    public class BingoViewModel : ReactiveObject
    {
        private readonly BingoCardSettings _settings;
        private readonly AudioPlaybackEngine _audioEngine;
        private CachedSound _winSound;
        private CachedSound _fillBallShortSound;
        private CachedSound _fillBallLongSound;
        private BingoBallViewModel[][] Matrix { get; set; }
        private BingoBallViewModel[][] MatrixForAnimation { get; set; }

        public BingoBallViewModel[] FlattendMatrix => Matrix.SelectMany(x=>x).ToArray();
        [Reactive] public BingoBallViewModel[] FlattendMatrixForAnimation { get; private set; }

        [Reactive] public bool IsAnimating { get; set; }
        public double HeightOfMatrix { get; set; }
        public double WidthOfMatrix { get; set; }
        
        public BingoViewModel(BingoCardSettings settings, Random random, AudioPlaybackEngine audioEngine)
        {
            _settings = settings;
            _audioEngine = audioEngine;
            _winSound = new CachedSound(@"Resources\Sounds\Bingo\slingo_win.wav");
            _fillBallShortSound = new CachedSound(@"Resources\Sounds\Bingo\ball_fill_short.wav");
            _fillBallLongSound = new CachedSound(@"Resources\Sounds\Bingo\ball_fill_long.wav");
            int[] numbers = Enumerable.Range(1, 50).Where(x => settings.EvenNumber ? ( x % 2 == 0 ) : (x % 2 != 0)).ToArray();
            Matrix = SetupMatrix(random, numbers, settings.FilledBalls);
            MatrixForAnimation = CopyMatrix(Matrix);
            
            IsAnimating = true;
        }

        public async Task FillInitialBalls()
        {
            await DropBallsFromTop();
            await Task.Delay(100);
            
            foreach (BingoBallViewModel[] bingoBallViewModels in Matrix)
            {
                foreach (BingoBallViewModel viewModel in bingoBallViewModels)
                {
                    if (_settings.FilledBalls.Contains(viewModel.Number))
                    {
                        viewModel.Fill();
                        _audioEngine.PlaySound(_fillBallShortSound);
                        await Task.Delay(100);
                    }
                }
            }
        }

        public async Task ClearBalls()
        {
            double height = MatrixForAnimation[0][0].Height;
            double width = MatrixForAnimation[0][0].Width;
            double horizontalMarginOfMatrix = 6;
            double verticalMarginOfMatrix = 3;

            double horizontalMargin = ((WidthOfMatrix - 2 * horizontalMarginOfMatrix) - width * MatrixForAnimation.Length) / (MatrixForAnimation.Length - 1);
            double verticalMargin = ((HeightOfMatrix - 2 * verticalMarginOfMatrix) - height * MatrixForAnimation[0].Length) / (MatrixForAnimation[0].Length - 1);
            horizontalMargin = Math.Ceiling(horizontalMargin);
            verticalMargin = Math.Ceiling(verticalMargin);

            // Get the columns
            BingoBallViewModel[][] columns = new BingoBallViewModel[MatrixForAnimation.Length][];
            double hMargin = horizontalMarginOfMatrix;
            for (int i = 0; i < MatrixForAnimation.Length; i++)
            {
                columns[i] = new BingoBallViewModel[MatrixForAnimation.Length];
                if (i > 0)
                {
                    hMargin += horizontalMargin;
                }
                
                double vMargin = verticalMarginOfMatrix;
                for (int j = 0; j < MatrixForAnimation[i].Length; j++)
                {
                    // Go down colum
                    columns[i][j] = MatrixForAnimation[j][i];
                    columns[i][j].X = hMargin + width * i;
                    columns[i][j].Y = vMargin + height * j;
                    vMargin += verticalMargin;
                }
            }

            FlattendMatrixForAnimation = MatrixForAnimation.SelectMany(x => x).ToArray();
            IsAnimating = true;
            await Task.Delay(10);
            
            // Drop balls
            int stepCounter = 0;
            int columnIndex = 0;
            int rowIndex = columns[0].Length - 1;
            List<AnimatedBall> balls = new List<AnimatedBall>();
            double maxY = HeightOfMatrix + height;

            while (true)
            {
                if (rowIndex > -1 && stepCounter++ > 2)
                {
                    stepCounter = 0;
                    balls.Add(new AnimatedBall(columns[columnIndex++][rowIndex], maxY, false, 1));

                    if (columnIndex > columns.Length - 1)
                    {
                        columnIndex = 0;
                        rowIndex--;
                    }
                }

                foreach (AnimatedBall animatedBall in balls)
                {
                    animatedBall.Step();
                }

                if (balls.Count == FlattendMatrixForAnimation.Length)
                {
                    if (balls.Last().Finished)
                    {
                        break;
                    }
                }

                await Task.Delay(10);
            }
        }

        private async Task DropBallsFromTop()
        {
            double height = MatrixForAnimation[0][0].Height;
            double width = MatrixForAnimation[0][0].Width;
            double horizontalMarginOfMatrix = 6;
            double verticalMarginOfMatrix = 3;
            
            double horizontalMargin = ((WidthOfMatrix  - 2 * horizontalMarginOfMatrix) - width * MatrixForAnimation.Length) / (MatrixForAnimation.Length -1);
            double verticalMargin   = ((HeightOfMatrix - 2 * verticalMarginOfMatrix)   - height * MatrixForAnimation[0].Length) / (MatrixForAnimation[0].Length -1);
            horizontalMargin = Math.Ceiling(horizontalMargin);
            verticalMargin = Math.Ceiling(verticalMargin);
            
            // Get the columns
            BingoBallViewModel[][] columns = new BingoBallViewModel[MatrixForAnimation.Length][];
            double margin = horizontalMarginOfMatrix;
            for (int i = 0; i < MatrixForAnimation.Length; i++)
            {
                columns[i] = new BingoBallViewModel[MatrixForAnimation.Length];
                if (i > 0)
                {
                    margin += horizontalMargin;
                }
                
                for (int j = 0; j < MatrixForAnimation[i].Length; j++)
                {
                    // Go down colum
                    columns[i][j] = MatrixForAnimation[j][i];
                    columns[i][j].X = margin + width * i;
                    columns[i][j].Y = -height;
                }
            }
            
            FlattendMatrixForAnimation =  MatrixForAnimation.SelectMany(x => x).ToArray();
            
            // Drop a single ball
            int stepCounter = 0;
            int columnIndex = 0;
            int rowIndex = columns[0].Length -1;
            List<AnimatedBall> balls = new List<AnimatedBall>();
            double maxY = (HeightOfMatrix - verticalMarginOfMatrix) - height;
            
            while (true)
            {
                if (rowIndex > -1 && stepCounter++ > 7)
                {
                    stepCounter = 0;
                    balls.Add(new AnimatedBall(columns[columnIndex++][rowIndex], maxY, true, 2.5));

                    if (columnIndex > columns.Length - 1)
                    {
                        columnIndex = 0;
                        rowIndex--;
                        maxY -= height + verticalMargin;
                    }
                }
                
                foreach (AnimatedBall animatedBall in balls)
                {
                    animatedBall.Step();
                }

                if (balls.Count == FlattendMatrixForAnimation.Length)
                {
                    if (balls.Last().Finished)
                    {
                        break;
                    }
                }

                await Task.Delay(10);
            }
            
            IsAnimating = false;
        }
        
        

        public async Task<bool> FillBall(int number)
        {
            foreach (BingoBallViewModel viewmodel in FlattendMatrix)
            {
                if(viewmodel.Number == number)
                {
                    _audioEngine.PlaySound(_fillBallLongSound);
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
            _audioEngine.PlaySound(_winSound);
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

        private BingoBallViewModel[][] CopyMatrix(BingoBallViewModel[][] matrix)
        {
            BingoBallViewModel[][] newMatrix = new BingoBallViewModel[matrix.Length][];
            for (var index = 0; index < matrix.Length; index++)
            {
                newMatrix[index] = new BingoBallViewModel[matrix[0].Length];
                for (int j = 0; j < matrix[index].Length; j++)
                {
                    newMatrix[index][j] = new BingoBallViewModel(matrix[index][j]);
                }
            }

            return newMatrix;
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
