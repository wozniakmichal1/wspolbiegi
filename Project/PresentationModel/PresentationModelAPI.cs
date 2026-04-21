using BusinessLogic;
using Data;

namespace PresentationModel
{
    public class PresentationModelAPI : PresentationModelAbstractAPI
    {
        private LogicAbstractAPI _logicAPI;

        public PresentationModelAPI(LogicAbstractAPI logicAPI)
        {
            _logicAPI = logicAPI;
        }

        public override IEnumerable<IBall> GetBalls() => _logicAPI.GetBalls();

        public override void MoveBalls()
        {
            _logicAPI.Step();
        }

        public override void StartSimulation(int BallCount, double BoardWidth, double BoardHight)
        {
            _logicAPI.StartSimulation(BallCount, BoardWidth, BoardHight);
        }

    }
}
