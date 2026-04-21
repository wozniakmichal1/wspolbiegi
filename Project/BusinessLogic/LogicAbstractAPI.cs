using Data;
namespace BusinessLogic
{
    public abstract class LogicAbstractAPI
    {

        public static LogicAbstractAPI CreateAPI(DataAbstractAPI api = null)
        {
            return new LogicAPI(api ?? DataAbstractAPI.CreateAPI());
        }

        public abstract double BoardWidth { get; }
        public abstract double BoardHeight { get; }

        public abstract IEnumerable<IBall> GetBalls();
        public abstract void StartSimulation(int BallCount, double BoardWidth, double BoardHeight);

        public abstract void Step();

    }
}
