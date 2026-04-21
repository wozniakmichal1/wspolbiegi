using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : IBall
    {
        private IVector _position;
        private double _diameter;

        public event PropertyChangedEventHandler? PropertyChanged;
        public double Diameter => _diameter;

        public IVector Position => _position;

        public Ball(double x, double y, double diameter)
        {
            _position = new Vector(x, y);
            _diameter = diameter;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propery = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propery));
        }

        public void Move(double x, double y)
        {
            _position = new Vector(x, y, this.Position.VelocityX, this.Position.VelocityY);
            OnPropertyChanged(nameof(Position));
        }

        public void InverseSpeed(string value)
        {
            if (value == "x")
            {
                _position = new Vector(this.Position.X, this.Position.Y, -1 * this.Position.VelocityX, this.Position.VelocityY);
            }
            else if (value == "y")
            {
                _position = new Vector(this.Position.X, this.Position.Y, this.Position.VelocityX, -1 * this.Position.VelocityY);
            }
            OnPropertyChanged(nameof(Position));
        }
    }
}
