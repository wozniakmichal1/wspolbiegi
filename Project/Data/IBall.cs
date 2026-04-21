using System.ComponentModel;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        IVector Position { get;}
        double Diameter { get; }

        void Move(double x, double y);
        public void InverseSpeed(string value);
    }
}
