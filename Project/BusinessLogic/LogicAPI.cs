using Data;

namespace BusinessLogic
{
    internal class LogicAPI : LogicAbstractAPI
    {
        private readonly DataAbstractAPI _dataApi;
        private double _boardWidth;
        private double _boardHeight;
        private CancellationTokenSource? _cts;

        private readonly object _collisionLock = new();

        public override double BoardWidth => _boardWidth;
        public override double BoardHeight => _boardHeight;
        public override event Action<IBall>? BallMoved;

        public LogicAPI(DataAbstractAPI dataApi)
        {
            _dataApi = dataApi;
        }

        public override IEnumerable<IBall> GetBalls() => _dataApi.GetBalls();

        public override void StartSimulation(int ballCount, double boardWidth, double boardHeight)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _dataApi.CreateBalls(ballCount, boardWidth, boardHeight);

            _cts = new CancellationTokenSource();

            var progress = new Progress<IBall>(OnBallMoved);

            _ = _dataApi.StartAsync(progress, _cts.Token);
        }

        public override void Stop() => _cts?.Cancel();

        
        private void OnBallMoved(IBall movedBall)
        {
            lock (_collisionLock)
            {
                var balls = _dataApi.GetBalls().ToList();
                HandleWallCollision(movedBall);
                HandleBallCollisions(balls);
            }

            
            BallMoved?.Invoke(movedBall);
        }

        private void HandleWallCollision(IBall ball)
        {
            double x = ball.Position.X;
            double y = ball.Position.Y;
            double vx = ball.Position.VelocityX;
            double vy = ball.Position.VelocityY;
            bool changed = false;

            if (x + ball.Diameter >= _boardWidth && vx > 0)
            { vx = -vx; changed = true; }
            else if (x <= 0 && vx < 0)
            { vx = -vx; changed = true; }

            if (y + ball.Diameter >= _boardHeight && vy > 0)
            { vy = -vy; changed = true; }
            else if (y <= 0 && vy < 0)
            { vy = -vy; changed = true; }

            if (changed)
                ball.SetVelocity(vx, vy);
        }

        private void HandleBallCollisions(List<IBall> balls)
        {
            var root = BVHTree.Build(balls);
            if (root == null) return;

            var candidates = new List<(IBall, IBall)>();
            root.CollectCandidatePairs(candidates);

            foreach (var (a, b) in candidates)
                ResolveCollision(a, b);
        }

        
        private static void ResolveCollision(IBall a, IBall b)
        {
            double ax = a.Position.X + a.Diameter / 2;
            double ay = a.Position.Y + a.Diameter / 2;
            double bx = b.Position.X + b.Diameter / 2;
            double by = b.Position.Y + b.Diameter / 2;

            double dx = bx - ax;
            double dy = by - ay;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            double minDist = (a.Diameter + b.Diameter) / 2;

            if (dist >= minDist || dist < 1e-10) return;

            double nx = dx / dist;
            double ny = dy / dist;

            double avx = a.Position.VelocityX;
            double avy = a.Position.VelocityY;
            double bvx = b.Position.VelocityX;
            double bvy = b.Position.VelocityY;

            double aDotN = avx * nx + avy * ny;
            double bDotN = bvx * nx + bvy * ny;

           
            if (aDotN - bDotN < 0) return;

            double ma = a.Mass;
            double mb = b.Mass;
            double massSum = ma + mb;

            double newADotN = (aDotN * (ma - mb) + 2 * mb * bDotN) / massSum;
            double newBDotN = (bDotN * (mb - ma) + 2 * ma * aDotN) / massSum;

            double deltaAN = newADotN - aDotN;
            double deltaBN = newBDotN - bDotN;

            a.SetVelocity(avx + deltaAN * nx, avy + deltaAN * ny);
            b.SetVelocity(bvx + deltaBN * nx, bvy + deltaBN * ny);
        }
    }
}