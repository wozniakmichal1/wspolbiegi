using BusinessLogic;
using Data;

namespace PresentationModel
{
    public class PresentationModelAPI : PresentationModelAbstractAPI
    {
        private readonly LogicAbstractAPI _logic;

        public override event Action<IBall>? BallMoved;

        public PresentationModelAPI(LogicAbstractAPI logic)
        {
            _logic = logic;
            _logic.BallMoved += ball => BallMoved?.Invoke(ball);
        }

        public override IEnumerable<IBall> GetBalls() => _logic.GetBalls();

        public override void StartSimulation(int ballCount, double boardWidth, double boardHeight)
            => _logic.StartSimulation(ballCount, boardWidth, boardHeight);

        public override void Stop() => _logic.Stop();
    }
}