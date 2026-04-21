using Data;
using PresentationModel;
using PresentationViewModel.MVVMLight;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace PresentationViewModel
{
    public class BoardViewModel : ViewModelBase
    {

        public double BoardWidth { get; }
        public double BoardHeight { get; }
        private ObservableCollection<IBall>? _balls;
        
        public ObservableCollection<IBall> Balls
        {
            get => _balls;
                set
            {
                _balls = value;
                RaisePropertyChanged();
            }
        }

        public BoardViewModel(PresentationModelAbstractAPI api, int BallCount, double width, double height)
        {
            this.BoardHeight = height;
            this.BoardWidth = width;
            Balls = new ObservableCollection<IBall>();
            api.StartSimulation(BallCount, BoardWidth, BoardHeight);

            IEnumerable<IBall> RawBalls = api.GetBalls();

            foreach (IBall ball in RawBalls)
            {
                Balls.Add(ball);
            }
            _ = RunSimulationAsync(api);
        }
        private async Task RunSimulationAsync(PresentationModelAbstractAPI api)
        {
            while (true)
            {
                api.MoveBalls();
                await Task.Delay(30);
            }
        }

    }
}
