using System;
using System.Collections.Generic;
using System.Text;

namespace Slingo.Game.Bingo
{
    public class AnimatedBall
    {
        private readonly BingoBallViewModel _ball;
        private readonly double _maxY;
        private readonly bool _shouldBounce;
        private readonly double _fallSpeedPerStep;
        private double _yVector;
        private double _y;

        public AnimatedBall(BingoBallViewModel ball, double maxY, bool shouldBounce, double fallSpeedPerStep)
        {
            _ball = ball;
            _maxY = maxY;
            _shouldBounce = shouldBounce;
            _fallSpeedPerStep = fallSpeedPerStep;
            _yVector = _fallSpeedPerStep;
            _y = _ball.Y;
        }

        public bool Finished { get; set; }

        public void Step()
        {
            double yNew = _y += _yVector;

            // When the bottom is reached
            if (yNew >= _maxY)
            {
                if (!_shouldBounce || _yVector < _fallSpeedPerStep * 3)
                {
                    // out of momentum
                    Finished =  true;
                    _ball.Y = _maxY;
                    return;
                }
                
                _yVector -= _fallSpeedPerStep * 5;
                _yVector = -_yVector;
                _y += _yVector;
                // surplus
                _y = _maxY;
                _ball.Y = _y; 
                return;
            }
            
            _y += _yVector;
            _ball.Y = _y;


            _yVector += _fallSpeedPerStep;
        }
    }
}
