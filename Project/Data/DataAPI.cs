using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataAPI : DataAbstractAPI
    {
        private List<IBall> _balls = new List<IBall>();

        public override void CreateBalls(int count, double MaxX, double MaxY)
        {
            _balls.Clear();
            var Random = new Random();
            for (int i = 0; i < count; i++)
            {
                double diameter = 20;
                double x = Random.NextDouble() * (MaxX - diameter);
                double y = Random.NextDouble() * (MaxY - diameter);
                _balls.Add(new Ball(x, y, diameter));
            }
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _balls;
        }
    }
}