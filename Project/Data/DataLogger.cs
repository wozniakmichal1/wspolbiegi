using System.Collections.Concurrent;

namespace Data
{
    public class DataLogger
    {
        protected BlockingCollection<String> BallData = new BlockingCollection<String>();

        private Task t1;

        public DataLogger()
        {
            t1 = Task.Run(() =>
            {
                using (StreamWriter sw = new StreamWriter("logi.csv", append: false))
                {
                    sw.WriteLine("X,Y,VelocityX,VelocityY,Diameter,Mass");
                    foreach (var item in BallData.GetConsumingEnumerable())
                    {
                        sw.WriteLine(item);
                    }
                    sw.Flush();
                }
            });
        }
        public void Add(String item)
        {
            BallData.Add(item);
        }
        public void Clear()
        {
            BallData.CompleteAdding();
        }

        public void WaitForCompletion()
        {
            t1.Wait();
        }
    }
}
