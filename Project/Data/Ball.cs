using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    internal class Ball : IBall
    {
        private readonly object _lock = new();
        private IVector _position;

        public double Diameter { get; }
        public double Mass { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public IVector Position
        {
            get { lock (_lock) { return _position; } }
        }

        public Ball(double x, double y, double diameter, double mass)
        {
            _position = new Vector(x, y);
            Diameter = diameter;
            Mass = mass;
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
            while (!token.IsCancellationRequested)
            {
                MoveStep();
                progress.Report(this);
                await Task.Delay(16, token);
            }
        }

        private void MoveStep()
        {
            lock (_lock)
            {
                _position = new Vector(
                    _position.X + _position.VelocityX,
                    _position.Y + _position.VelocityY,
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