using Data;

namespace DataTests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void Vector_RandomConstructor_SetsPosition()
        {
            var v = CreateVector(10, 20);
            Assert.AreEqual(10, v.X, 0.001);
            Assert.AreEqual(20, v.Y, 0.001);
        }

        [TestMethod]
        public void Vector_RandomConstructor_VelocityNonZero()
        {
            var v = CreateVector(0, 0);
            Assert.IsTrue(Math.Abs(v.VelocityX) >= 20 && Math.Abs(v.VelocityX) <= 80);
            Assert.IsTrue(Math.Abs(v.VelocityY) >= 20 && Math.Abs(v.VelocityY) <= 80);
        }

        [TestMethod]
        public void Vector_FullConstructor_SetsAllValues()
        {
            var v = CreateVector(5, 10, 3, -2);
            Assert.AreEqual(5, v.X, 0.001);
            Assert.AreEqual(10, v.Y, 0.001);
            Assert.AreEqual(3, v.VelocityX, 0.001);
            Assert.AreEqual(-2, v.VelocityY, 0.001);
        }

        [TestMethod]
        public void Vector_FullConstructor_NegativeVelocity()
        {
            var v = CreateVector(0, 0, -5, -5);
            Assert.AreEqual(-5, v.VelocityX, 0.001);
            Assert.AreEqual(-5, v.VelocityY, 0.001);
        }

        [TestMethod]
        public void Vector_FullConstructor_ZeroVelocity()
        {
            var v = CreateVector(10, 10, 0, 0);
            Assert.AreEqual(0, v.VelocityX, 0.001);
            Assert.AreEqual(0, v.VelocityY, 0.001);
        }

        private static IVector CreateVector(double x, double y)
        {
            return (IVector)typeof(DataAbstractAPI).Assembly
                .CreateInstance("Data.Vector", false,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    null, new object[] { x, y }, null, null)!;
        }

        private static IVector CreateVector(double x, double y, double vx, double vy)
        {
            return (IVector)typeof(DataAbstractAPI).Assembly
                .CreateInstance("Data.Vector", false,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    null, new object[] { x, y, vx, vy }, null, null)!;
        }
    }

    [TestClass]
    public class BallTests
    {

        [TestMethod]
        public void DataAPI_CreateBalls_ReturnsCorrectCount()
        {
            var api = DataAbstractAPI.CreateAPI();
            api.CreateBalls(5, 300, 300);
            Assert.AreEqual(5, api.GetBalls().Count());
        }

        [TestMethod]
        public void DataAPI_CreateBalls_BallsWithinBounds()
        {
            var api = DataAbstractAPI.CreateAPI();
            api.CreateBalls(10, 300, 300);
            foreach (var ball in api.GetBalls())
            {
                Assert.IsTrue(ball.Position.X >= 0 && ball.Position.X + ball.Diameter <= 300);
                Assert.IsTrue(ball.Position.Y >= 0 && ball.Position.Y + ball.Diameter <= 300);
            }
        }

        [TestMethod]
        public void DataAPI_CreateBalls_ReplacesExisting()
        {
            var api = DataAbstractAPI.CreateAPI();
            api.CreateBalls(5, 300, 300);
            api.CreateBalls(3, 300, 300);
            Assert.AreEqual(3, api.GetBalls().Count());
        }

        [TestMethod]
        public void DataAPI_CreateBalls_DiameterInExpectedRange()
        {
            var api = DataAbstractAPI.CreateAPI();
            api.CreateBalls(20, 500, 500);
            foreach (var ball in api.GetBalls())
                Assert.IsTrue(ball.Diameter >= 15 && ball.Diameter <= 35);
        }

        [TestMethod]
        public void DataAPI_CreateBalls_MassIsPositive()
        {
            var api = DataAbstractAPI.CreateAPI();
            api.CreateBalls(5, 300, 300);
            foreach (var ball in api.GetBalls())
                Assert.IsTrue(ball.Mass > 0);
        }

        [TestMethod]
        public async Task DataAPI_StartAsync_BallsStartMoving()
        {
            var api = DataAbstractAPI.CreateAPI();
            api.CreateBalls(3, 300, 300);
            var positions = api.GetBalls().Select(b => (b.Position.X, b.Position.Y)).ToList();
            var cts = new CancellationTokenSource(200);
            try { await api.StartAsync(new Progress<IBall>(_ => { }), cts.Token); }
            catch (OperationCanceledException) { }
            var newPositions = api.GetBalls().Select(b => (b.Position.X, b.Position.Y)).ToList();
            bool anyMoved = positions.Zip(newPositions, (a, b) => a != b).Any(m => m);
            Assert.IsTrue(anyMoved);
        }

    
    }
}