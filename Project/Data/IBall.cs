using System.ComponentModel;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        IVector Position { get; }
        double Diameter { get; }
        double Mass { get; }

        void SetVelocity(double vx, double vy);

        Task StartMovingAsync(IProgress<IBall> progress, CancellationToken token);
    }
}