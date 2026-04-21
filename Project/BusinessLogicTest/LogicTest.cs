using BusinessLogic;
using Data;

namespace BusinessLogicTests
{
    [TestClass]
    public class LogicAPITests
    {
        private const double BoardWidth = 300;
        private const double BoardHeight = 300;

        private LogicAbstractAPI CreateLogic()
        {
            return LogicAbstractAPI.CreateAPI();
        }

       
        [TestMethod]
        public void StartSimulation_SetsBoardWidth()
        {
            var logic = CreateLogic();
            logic.StartSimulation(1, BoardWidth, BoardHeight);

            Assert.AreEqual(BoardWidth, logic.BoardWidth);
        }

        [TestMethod]
        public void StartSimulation_SetsBoardHeight()
        {
            var logic = CreateLogic();
            logic.StartSimulation(1, BoardWidth, BoardHeight);

            Assert.AreEqual(BoardHeight, logic.BoardHeight);
        }


        [TestMethod]
        public void GetBalls_AfterStartSimulation_ReturnsCorrectCount()
        {
            var logic = CreateLogic();
            logic.StartSimulation(5, BoardWidth, BoardHeight);

            Assert.AreEqual(5, logic.GetBalls().Count());
        }

        [TestMethod]
        public void GetBalls_BeforeStartSimulation_ReturnsEmpty()
        {
            var logic = CreateLogic();

            Assert.AreEqual(0, logic.GetBalls().Count());
        }

        [TestMethod]
        public void GetBalls_BallsSpawnedWithinBoard()
        {
            var logic = CreateLogic();
            logic.StartSimulation(10, BoardWidth, BoardHeight);

            foreach (var ball in logic.GetBalls())
            {
                Assert.IsTrue(ball.Position.X >= 0 && ball.Position.X + ball.Diameter <= BoardWidth);
                Assert.IsTrue(ball.Position.Y >= 0 && ball.Position.Y + ball.Diameter <= BoardHeight);
            }
        }


        [TestMethod]
        public void Step_BallsStayWithinBoardAfterMultipleSteps()
        {
            var logic = CreateLogic();
            logic.StartSimulation(5, BoardWidth, BoardHeight);

            for (int i = 0; i < 100; i++)
                logic.Step();

            foreach (var ball in logic.GetBalls())
            {
                Assert.IsTrue(ball.Position.X >= 0);
                Assert.IsTrue(ball.Position.X + ball.Diameter <= BoardWidth);
                Assert.IsTrue(ball.Position.Y >= 0);
                Assert.IsTrue(ball.Position.Y + ball.Diameter <= BoardHeight);
            }
        }

        [TestMethod]
        public void Step_BallsChangePosition()
        {
            var logic = CreateLogic();
            logic.StartSimulation(3, BoardWidth, BoardHeight);

            var before = logic.GetBalls()
                .Select(b => (b.Position.X, b.Position.Y))
                .ToList();

            logic.Step();

            var after = logic.GetBalls()
                .Select(b => (b.Position.X, b.Position.Y))
                .ToList();

            bool anyMoved = before.Zip(after, (b, a) => b != a).Any(moved => moved);
            Assert.IsTrue(anyMoved);
        }

    }
}