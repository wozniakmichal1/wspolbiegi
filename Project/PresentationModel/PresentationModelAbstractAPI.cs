using BusinessLogic;
using Data;

namespace PresentationModel
{
    public abstract class PresentationModelAbstractAPI
    {
        public static PresentationModelAbstractAPI CreateAPI(LogicAbstractAPI? api = null)
        {
            return new PresentationModelAPI(api ?? LogicAbstractAPI.CreateAPI());
        }

        public abstract void StartSimulation(int BallCount, double BoardWidth, double BoardHight);

        public abstract IEnumerable<IBall> GetBalls();

        public abstract void MoveBalls();
    }
}
