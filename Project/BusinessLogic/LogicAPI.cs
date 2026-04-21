using Data;

namespace BusinessLogic
{
    public class LogicAPI : LogicAbstractAPI
    {
        private DataAbstractAPI _dataApi;
        private double _boardWidth;
        private double _boardHeight;

        public override double BoardWidth => _boardWidth;
        public override double BoardHeight => _boardHeight;

        public LogicAPI(DataAbstractAPI DataApi)
        {
            _dataApi = DataApi;
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _dataApi.GetBalls();
        }
        public override void StartSimulation(int BallCount, double BoardWidth, double BoardHight)
        {
            _dataApi.CreateBalls(BallCount,BoardWidth,BoardHight);
            _boardHeight = BoardHight;
            _boardWidth = BoardWidth;
        }

        public override void Step()
        {
            Random random = new Random();
            IEnumerable<IBall> balls = _dataApi.GetBalls();
            foreach (IBall ball in balls)
            {
                
                double newX = ball.Position.X + ball.Position.VelocityX;
                double newY = ball.Position.Y + ball.Position.VelocityY;

                if (ball.Diameter + newX > _boardWidth)
                {
                    newX = _boardWidth - ball.Diameter;
                }
                else if (newX < 0)
                {
                    newX = 0;
                }

                if (ball.Diameter + newY > _boardHeight)
                {
                    newY = _boardHeight - ball.Diameter;
                }
                else if (newY < 0)
                {
                    newY = 0;
                }

                ball.Move(newX, newY);

                if (ball.Diameter + newX >= _boardWidth || newX == 0)
                {
                    ball.InverseSpeed("x");
                }

                if (ball.Diameter + newY >= _boardHeight || newY == 0)
                {
                    ball.InverseSpeed("y");
                }
               
            }
        }
    }
}
