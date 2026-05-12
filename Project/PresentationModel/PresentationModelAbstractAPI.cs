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

        public abstract void StartSimulation(int ballCount, double boardWidth, double boardHeight);
        public abstract void Stop();
        public abstract IEnumerable<IBall> GetBalls();

        
        public abstract event Action<IBall>? BallMoved;
    }
}