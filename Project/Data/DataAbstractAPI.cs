namespace Data
{
    public abstract class DataAbstractAPI
    {
         

        public static DataAbstractAPI CreateAPI()
        {
            return new DataAPI();
        }

        public abstract void CreateBalls(int count, double MaxX, double MaxY);
        public abstract IEnumerable<IBall> GetBalls();
    }
}
