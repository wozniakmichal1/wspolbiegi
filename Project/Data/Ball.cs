using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Data
{
    internal class Ball : IBall
    {
        private DataLogger _logger;
        private readonly object _lock = new();
        private IVector _position;

        public double Diameter { get; }
        public double Mass { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public IVector Position
        {
            get { lock (_lock) { return _position; } }
        }

        public Ball(double x, double y, double diameter, double mass, DataLogger logger)
        {
            _position = new Vector(x, y);
            Diameter = diameter;
            Mass = mass;
            _logger = logger;
        }

        public void SetVelocity(double vx, double vy)
        {
            lock (_lock)
            {
                _position = new Vector(_position.X, _position.Y, vx, vy);
            }
        }

        public async Task StartMovingAsync(IProgress<IBall> progress, CancellationToken token)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (!token.IsCancellationRequested)
            {
                double DeltaTime = sw.Elapsed.TotalSeconds;
                sw.Restart();
                MoveStep(DeltaTime);
                progress.Report(this);
                String PosString = _position.X.ToString() + "," + _position.Y.ToString() + ",";
                String VelString = _position.VelocityX.ToString() + "," + _position.VelocityY.ToString() + ",";
                String BallInfoString = Diameter.ToString() + "," + Mass.ToString();
                _logger.Add(PosString + VelString + BallInfoString);
                try
                {
                    await Task.Delay(10, token);
                }
                catch (TaskCanceledException)
                { 
                    break;
                }
            }
        }

        private void MoveStep(double DeltaTime)
        {
            lock (_lock)
            {
                _position = new Vector(
                    _position.X + _position.VelocityX * DeltaTime,
                    _position.Y + _position.VelocityY * DeltaTime,
                    _position.VelocityX,
                    _position.VelocityY);
            }
            OnPropertyChanged(nameof(Position));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}