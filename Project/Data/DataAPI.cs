namespace Data
{
    internal class DataAPI : DataAbstractAPI
    {
        private readonly List<IBall> _balls = new();
        private readonly object _ballsLock = new();

        public override void CreateBalls(int count, double maxX, double maxY)
        {
            lock (_ballsLock)
            {
                _balls.Clear();
                var rng = new Random();
                for (int i = 0; i < count; i++)
                {
                    double diameter = rng.NextDouble() * 20 + 15;
                    double mass = Math.PI * Math.Pow(diameter / 2, 2);
                    double x = rng.NextDouble() * (maxX - diameter);
                    double y = rng.NextDouble() * (maxY - diameter);
                    _balls.Add(new Ball(x, y, diameter, mass));
                }
            }
        }

        public override IEnumerable<IBall> GetBalls()
        {
            lock (_ballsLock) { return _balls.ToList(); }
        }

        public override Task StartAsync(IProgress<IBall> progress, CancellationToken token)
        {
            List<IBall> snapshot;
            lock (_ballsLock) { snapshot = _balls.ToList(); }

            var tasks = snapshot.Select(b => b.StartMovingAsync(progress, token));
            return Task.WhenAll(tasks);
        }
    }
}