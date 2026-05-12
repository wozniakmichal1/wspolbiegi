using PresentationViewModel;
using PresentationModel;
using Data;
using System.ComponentModel;

namespace PresentationViewModelTest
{
    // --- KLASY ZASTĘPCZE (FAKES) ---

    public class FakeBall : IBall
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public IVector Position { get; set; } = default!;
        public double Diameter { get; set; }
        public double Mass { get; set; }

        public void SetVelocity(double vx, double vy)
        {
            // No-op for fake
        }

        public Task StartMovingAsync(IProgress<IBall> progress, CancellationToken token)
        {
            // No-op for fake
            return Task.CompletedTask;
        }
    }

    public class FakePresentationModelAPI : PresentationModelAbstractAPI
    {
        public bool StartSimulationCalled { get; private set; }
        public int BallCount { get; private set; }
        public double BoardWidth { get; private set; }
        public double BoardHeight { get; private set; }
        public bool StopCalled { get; private set; }

        public List<IBall> BallsToReturn { get; set; } = new List<IBall>();

        public override event Action<IBall>? BallMoved;

        public override void StartSimulation(int ballCount, double boardWidth, double boardHeight)
        {
            StartSimulationCalled = true;
            BallCount = ballCount;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
        }

        public override void Stop() => StopCalled = true;
        public override IEnumerable<IBall> GetBalls() => BallsToReturn;
    }

    // --- TESTY ---

    [TestClass]
    public class MainWindowViewModelTests
    {
        [TestMethod]
        public void Constructor_ShouldSetDefaultValues()
        {
            // Arrange
            var fakeApi = new FakePresentationModelAPI();

            // Act
            var viewModel = new MainWindowViewModel(fakeApi);

            // Assert
            Assert.AreEqual("5", viewModel.BallNumberInput);
            Assert.AreEqual(viewModel, viewModel.CurrentViewModel);
            Assert.IsNotNull(viewModel.StartCommand);
        }

        [TestMethod]
        public void StartCommand_Execute_ShouldCreateBoardViewModelAndChangeCurrentViewModel()
        {
            // Arrange
            var fakeApi = new FakePresentationModelAPI();
            var viewModel = new MainWindowViewModel(fakeApi)
            {
                BallNumberInput = "10"
            };

            // Act
            viewModel.StartCommand.Execute(null);

            // Assert
            Assert.IsInstanceOfType(viewModel.CurrentViewModel, typeof(BoardViewModel));
            var boardViewModel = (BoardViewModel)viewModel.CurrentViewModel;

            Assert.AreEqual(400, boardViewModel.BoardWidth);
            Assert.AreEqual(350, boardViewModel.BoardHeight);

            Assert.IsTrue(fakeApi.StartSimulationCalled);
            Assert.AreEqual(10, fakeApi.BallCount);
            Assert.AreEqual(400, fakeApi.BoardWidth);
            Assert.AreEqual(350, fakeApi.BoardHeight);
        }
    }

    [TestClass]
    public class BoardViewModelTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializePropertiesAndStartSimulation()
        {
            // Arrange
            var fakeApi = new FakePresentationModelAPI();
            fakeApi.BallsToReturn = new List<IBall> { new FakeBall(), new FakeBall() };

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            // Act
            var viewModel = new BoardViewModel(fakeApi, 2, 500, 400);

            // Assert
            Assert.AreEqual(500, viewModel.BoardWidth);
            Assert.AreEqual(400, viewModel.BoardHeight);
            Assert.AreEqual(2, viewModel.Balls.Count);

            Assert.IsTrue(fakeApi.StartSimulationCalled);
            Assert.AreEqual(2, fakeApi.BallCount);
        }

        [TestMethod]
        public void Stop_ShouldCallApiStop()
        {
            // Arrange
            var fakeApi = new FakePresentationModelAPI();
            var viewModel = new BoardViewModel(fakeApi, 1, 100, 100);

            // Act
            viewModel.Stop();

            // Assert
            Assert.IsTrue(fakeApi.StopCalled);
        }
    }
}