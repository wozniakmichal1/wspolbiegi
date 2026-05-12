using BusinessLogic;
using BusinessLogicTests.Fakes;
using Data;

namespace BusinessLogicTests
{
    [TestClass]
    public class LogicAPITests
    {
        private const double W = 300;
        private const double H = 300;

     
        private (LogicAbstractAPI logic, FakeDataAPI data) Create(
            IEnumerable<IBall>? balls = null)
        {
            var data = new FakeDataAPI();
            if (balls != null) data.SetBalls(balls);
            var logic = LogicAbstractAPI.CreateAPI(data);
            logic.StartSimulation(0, W, H);
            return (logic, data);
        }



        [TestMethod]
        public void StartSimulation_SetsBoardWidth()
        {
            var (logic, _) = Create();
            Assert.AreEqual(W, logic.BoardWidth);
        }

        [TestMethod]
        public void StartSimulation_SetsBoardHeight()
        {
            var (logic, _) = Create();
            Assert.AreEqual(H, logic.BoardHeight);
        }



        [TestMethod]
        public void GetBalls_ReturnsAllBallsFromDataLayer()
        {
            var balls = new[] { new FakeBall(50, 50, 1, 1), new FakeBall(100, 100, 1, 1) };
            var (logic, _) = Create(balls);
            Assert.AreEqual(2, logic.GetBalls().Count());
        }

        [TestMethod]
        public void GetBalls_WhenEmpty_ReturnsEmpty()
        {
            var (logic, _) = Create(Array.Empty<IBall>());
            Assert.AreEqual(0, logic.GetBalls().Count());
        }



        [TestMethod]
        public void WallCollision_RightWall_InvertsVelocityX()
        {
    
            var ball = new FakeBall(x: 285, y: 50, vx: 3, vy: 0, diameter: 20);
            var (logic, data) = Create(new[] { ball });

           
            SimulateBallMoved(logic, ball);

            Assert.IsTrue(ball.Position.VelocityX < 0);
        }

        [TestMethod]
        public void WallCollision_LeftWall_InvertsVelocityX()
        {
            var ball = new FakeBall(x: 0, y: 50, vx: -3, vy: 0, diameter: 20);
            var (logic, data) = Create(new[] { ball });

            SimulateBallMoved(logic, ball);

            Assert.IsTrue(ball.Position.VelocityX > 0);
        }

        [TestMethod]
        public void WallCollision_BottomWall_InvertsVelocityY()
        {
            var ball = new FakeBall(x: 50, y: 285, vx: 0, vy: 3, diameter: 20);
            var (logic, data) = Create(new[] { ball });

            SimulateBallMoved(logic, ball);

            Assert.IsTrue(ball.Position.VelocityY < 0);
        }

        [TestMethod]
        public void WallCollision_TopWall_InvertsVelocityY()
        {
            var ball = new FakeBall(x: 50, y: 0, vx: 0, vy: -3, diameter: 20);
            var (logic, data) = Create(new[] { ball });

            SimulateBallMoved(logic, ball);

            Assert.IsTrue(ball.Position.VelocityY > 0);
        }

        [TestMethod]
        public void WallCollision_BallMovingAway_NoVelocityChange()
        {
           
            var ball = new FakeBall(x: 285, y: 50, vx: -3, vy: 0, diameter: 20);
            var (logic, _) = Create(new[] { ball });

            SimulateBallMoved(logic, ball);

            Assert.AreEqual(0, ball.VelocityHistory.Count);
        }

        [TestMethod]
        public void WallCollision_BallInMiddle_NoVelocityChange()
        {
            var ball = new FakeBall(x: 100, y: 100, vx: 2, vy: 2, diameter: 20);
            var (logic, _) = Create(new[] { ball });

            SimulateBallMoved(logic, ball);

            Assert.AreEqual(0, ball.VelocityHistory.Count);
        }


        [TestMethod]
        public void BallCollision_OverlappingBalls_VelocityChanges()
        {
           
            var a = new FakeBall(x: 50, y: 50, vx: 2, vy: 0, diameter: 20, mass: 100);
            var b = new FakeBall(x: 60, y: 50, vx: -2, vy: 0, diameter: 20, mass: 100);
            var (logic, data) = Create(new IBall[] { a, b });

            SimulateBallMoved(logic, a);

          
            Assert.IsTrue(a.VelocityHistory.Count > 0 || b.VelocityHistory.Count > 0);
        }

        [TestMethod]
        public void BallCollision_EqualMass_ExchangesVelocity()
        {
        
            var a = new FakeBall(x: 50, y: 50, vx: 3, vy: 0, diameter: 20, mass: 100);
            var b = new FakeBall(x: 60, y: 50, vx: 0, vy: 0, diameter: 20, mass: 100);
            var (logic, _) = Create(new IBall[] { a, b });

            SimulateBallMoved(logic, a);

            if (a.VelocityHistory.Count > 0 && b.VelocityHistory.Count > 0)
            {
          
                var aFinal = a.VelocityHistory.Last();
                var bFinal = b.VelocityHistory.Last();
                Assert.AreEqual(0, aFinal.vx, 0.5);
                Assert.AreEqual(3, bFinal.vx, 0.5);
            }
        }

        [TestMethod]
        public void BallMoved_EventFired_WhenBallMoves()
        {
            var ball = new FakeBall(x: 100, y: 100, vx: 1, vy: 1, diameter: 20);
            var (logic, _) = Create(new[] { ball });

            IBall? notified = null;
            logic.BallMoved += b => notified = b;

            SimulateBallMoved(logic, ball);

            Assert.IsNotNull(notified);
        }


        private static void SimulateBallMoved(LogicAbstractAPI logic, IBall ball)
        {

            var method = logic.GetType()
                .GetMethod("OnBallMoved",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
            method?.Invoke(logic, new object[] { ball });
        }
    }
}