namespace Data
{
    public abstract class DataAbstractAPI
    {
        public static DataAbstractAPI CreateAPI()
        {
            return new DataAPI();
        }

        public abstract void CreateBalls(int count, double maxX, double maxY);
        public abstract IEnumerable<IBall> GetBalls();

        public abstract Task StartAsync(IProgress<IBall> progress, CancellationToken token);
    }
}