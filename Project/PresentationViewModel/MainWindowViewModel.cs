using PresentationViewModel.MVVMLight;
using PresentationModel;
using System.Windows.Input;

namespace PresentationViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private PresentationModelAbstractAPI _api;
        private ViewModelBase? _currentViewModel;
        private string _ballNumberInput = "5";
        public string BallNumberInput
        {
            get { return _ballNumberInput; } set { _ballNumberInput = value; RaisePropertyChanged(); }
        }
        public ICommand StartCommand { get; }

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel() : this(null) { }
        public MainWindowViewModel(PresentationModelAbstractAPI api)
        {
            _api = api ?? PresentationModelAbstractAPI.CreateAPI();
            CurrentViewModel = this;
            StartCommand = new RelayCommand(StartSimulation);
        }
        private void StartSimulation()
        {
            int count = int.Parse(BallNumberInput);

            var BoardScene = new BoardViewModel(_api, count, 400, 350);

            CurrentViewModel = BoardScene;
        }

    }
}
