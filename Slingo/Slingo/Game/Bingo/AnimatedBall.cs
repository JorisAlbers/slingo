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
        private double _y;

        public AnimatedBall(BingoBallViewModel ball, double maxY)
        {
            _ball = ball;
            _maxY = maxY;
            _y = _ball.Y;
        }

        public bool Finished { get; set; }

        public void Step()
        {
            double yNew = _y += _yVector;

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
                
                _yVector -= _FALL_SPEED_PER_STEP * 5;
                _yVector = -_yVector;
                _y += _yVector;
                // surplus
                _y = _maxY;
                _ball.Y = _y; 
                return;
            }
            
            _y += _yVector;
            _ball.Y = _y;


            _yVector += _FALL_SPEED_PER_STEP;
        }
    }
}
