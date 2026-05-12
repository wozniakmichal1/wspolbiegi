namespace Data
{
    internal class Vector : IVector
    {
        public double X { get; }
        public double Y { get; }
        public double VelocityX { get; }
        public double VelocityY { get; }

        internal Vector(double x, double y)
        {
            var rng = new Random();
            X = x;
            Y = y;
            VelocityX = (rng.NextDouble() * 3 + 1) * (rng.Next(2) == 0 ? 1 : -1);
            VelocityY = (rng.NextDouble() * 3 + 1) * (rng.Next(2) == 0 ? 1 : -1);
        }

        internal Vector(double x, double y, double vx, double vy)
        {
            X = x; Y = y; VelocityX = vx; VelocityY = vy;
        }
    }
}