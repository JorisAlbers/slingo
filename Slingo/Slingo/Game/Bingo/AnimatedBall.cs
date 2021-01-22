using System;
using System.Collections.Generic;
using System.Text;

namespace Slingo.Game.Bingo
{
    public class AnimatedBall
    {
        private const double _FALL_SPEED_PER_STEP = 2.5;
        private readonly BingoBallViewModel _ball;
        private readonly double _maxY;
        private double _yVector = _FALL_SPEED_PER_STEP;

        public AnimatedBall(BingoBallViewModel ball, double maxY)
        {
            _ball = ball;
            _maxY = maxY;
            Y = _ball.Y;
        }
        
        public double Y { get; set; }
        public bool Finished { get; set; }

        public void Step()
        {
            double yNew = Y += _yVector;

            // When the bottom is reached
            if (yNew >= _maxY)
            {
                if (_yVector < _FALL_SPEED_PER_STEP * 3)
                {
                    // out of momentum
                    Finished =  true;
                    _ball.Y = _maxY;
                    return;
                }
                
                _yVector -= _FALL_SPEED_PER_STEP * 3;
                _yVector = -_yVector;
                Y += _yVector;
                // surplus
                Y = _maxY;
                _ball.Y = Y; 
                return;
            }
            
            Y += _yVector;
            _ball.Y = Y;


            _yVector += _FALL_SPEED_PER_STEP;
        }
    }
}
