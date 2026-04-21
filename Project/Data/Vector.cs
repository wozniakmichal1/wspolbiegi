namespace Data
{
    internal class Vector : IVector
    {
        private double _x;
        private double _y;
        private double _velocityX;
        private double _velocityY;
           
        public Vector(double x, double y) { 
            Random random = new Random();
            _x = x;
            _y = y;
            _velocityX = random.NextDouble() * random.Next(1,5);
            _velocityY = random.NextDouble() * random.Next(1,5);
        }
        internal Vector(double x, double y, double VelocityX, double VelocityY)
        {
            _x = x;
            _y = y;
            _velocityX = VelocityX;
            _velocityY = VelocityY;
        }
        public double X => _x;
        public double Y => _y;

        public double VelocityX => _velocityX;
        public double VelocityY => _velocityY;
    }
}
