using Data;
using System.ComponentModel;

namespace BusinessLogicTests.Fakes
{
   
    internal class FakeVector : IVector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        public FakeVector(double x, double y, double vx, double vy)
        { X = x; Y = y; VelocityX = vx; VelocityY = vy; }
    }

  
    internal class FakeBall : IBall
    {
        private IVector _position;

        public double Diameter { get; }
        public double Mass { get; }
        public IVector Position => _position;

        public List<(double vx, double vy)> VelocityHistory { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public FakeBall(double x, double y, double vx, double vy,
                        double diameter = 20, double mass = 100)
        {
            _position = new FakeVector(x, y, vx, vy);
            Diameter = diameter;
            Mass = mass;
        }

        public void SetVelocity(double vx, double vy)
        {
            VelocityHistory.Add((vx, vy));
            _position = new FakeVector(_position.X, _position.Y, vx, vy);
        }

      
        public Task StartMovingAsync(IProgress<IBall> progress, CancellationToken token)
            => Task.CompletedTask;
    }

    internal class FakeDataAPI : DataAbstractAPI
    {
        private List<IBall> _balls = new();

        public void SetBalls(IEnumerable<IBall> balls)
            => _balls = balls.ToList();

        public override void CreateBalls(int count, double maxX, double maxY)
        {}

        public override IEnumerable<IBall> GetBalls() => _balls.ToList();

        public override Task StartAsync(IProgress<IBall> progress, CancellationToken token)
            => Task.CompletedTask;
    }
}