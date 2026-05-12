using PresentationModel;
using BusinessLogic;
using Data;
using System.ComponentModel;
namespace PresentationModelTest
{

    public class FakeBall : IBall
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public IVector Position { get; set; } = null!;
        public double Diameter { get; set; }
        public double Mass { get; set; }

        public void SetVelocity(double vx, double vy)
        {
            // Fake implementation does nothing
        }

        public Task StartMovingAsync(IProgress<IBall> progress, CancellationToken token)
        {
            // Fake implementation returns completed task
            return Task.CompletedTask;
        }
    }

    public class FakeLogicAPI : LogicAbstractAPI
    {
        public bool StartSimulationCalled { get; private set; }
        public int BallCount { get; private set; }
        private double _boardWidth;
        private double _boardHeight;
        public bool StopCalled { get; private set; }

        public List<IBall> BallsToReturn { get; set; } = new List<IBall>();

        public override double BoardWidth => _boardWidth;

        public override double BoardHeight => _boardHeight;

        public override event Action<IBall>? BallMoved;

        public void RaiseBallMoved(IBall ball) => BallMoved?.Invoke(ball);

        public override void StartSimulation(int ballCount, double boardWidth, double boardHeight)
        {
            StartSimulationCalled = true;
            BallCount = ballCount;
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
        }

        public override void Stop() => StopCalled = true;
        public override IEnumerable<IBall> GetBalls() => BallsToReturn;
    }

    // --- TESTY ---

    [TestClass]
    public class PresentationModelAPITests
    {
        private FakeLogicAPI _fakeLogicApi;
        private PresentationModelAPI _presentationModelApi;

        [TestInitialize]
        public void Setup()
        {
            _fakeLogicApi = new FakeLogicAPI();
            _presentationModelApi = new PresentationModelAPI(_fakeLogicApi);
        }

        [TestMethod]
        public void StartSimulation_ShouldCallLogicAPI_StartSimulation()
        {
            // Act
            _presentationModelApi.StartSimulation(5, 400, 300);

            // Assert
            Assert.IsTrue(_fakeLogicApi.StartSimulationCalled);
            Assert.AreEqual(5, _fakeLogicApi.BallCount);
            Assert.AreEqual(400, _fakeLogicApi.BoardWidth);
            Assert.AreEqual(300, _fakeLogicApi.BoardHeight);
        }

        [TestMethod]
        public void Stop_ShouldCallLogicAPI_Stop()
        {
            // Act
            _presentationModelApi.Stop();

            // Assert
            Assert.IsTrue(_fakeLogicApi.StopCalled);
        }

        [TestMethod]
        public void GetBalls_ShouldReturnBallsFromLogicAPI()
        {
            // Arrange
            var expectedBalls = new List<IBall> { new FakeBall() };
            _fakeLogicApi.BallsToReturn = expectedBalls;

            // Act
            var result = _presentationModelApi.GetBalls();

            // Assert
            Assert.AreEqual(expectedBalls, result);
        }

        [TestMethod]
        public void BallMoved_EventShouldBeInvoked_WhenLogicAPIRaisesEvent()
        {
            // Arrange
            var fakeBall = new FakeBall();
            bool eventRaised = false;

            _presentationModelApi.BallMoved += (ball) =>
            {
                eventRaised = true;
                Assert.AreEqual(fakeBall, ball);
            };

            // Act
            _fakeLogicApi.RaiseBallMoved(fakeBall);

            // Assert
            Assert.IsTrue(eventRaised, "Zdarzenie BallMoved nie zostało wywołane.");
        }
    }
}