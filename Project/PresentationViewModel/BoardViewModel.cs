using Data;
using PresentationModel;
using PresentationViewModel.MVVMLight;
using System.Collections.ObjectModel;

namespace PresentationViewModel
{
    public class BoardViewModel : ViewModelBase
    {
        public double BoardWidth { get; }
        public double BoardHeight { get; }

        private ObservableCollection<IBall> _balls = new();
        public ObservableCollection<IBall> Balls
        {
            get => _balls;
            set { _balls = value; RaisePropertyChanged(); }
        }

        private readonly PresentationModelAbstractAPI _api;

       
        private readonly SynchronizationContext _uiContext;

        public BoardViewModel(PresentationModelAbstractAPI api, int ballCount,
                              double width, double height)
        {
            _api = api;
            BoardWidth = width;
            BoardHeight = height;

            _uiContext = SynchronizationContext.Current
                         ?? new SynchronizationContext();

            api.StartSimulation(ballCount, BoardWidth, BoardHeight);

            foreach (var ball in api.GetBalls())
                Balls.Add(ball);

            api.BallMoved += OnBallMoved;
        }

        private void OnBallMoved(IBall ball)
        {
            _uiContext.Post(_ => {}, null);
        }

        public void Stop() => _api.Stop();
    }
}