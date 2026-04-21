using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationModel;
using BusinessLogic;
using Data;
using System.Linq;

namespace PresentationModelTest
{
    [TestClass]
    public class PresentationModelTests
    {
      
        private class TestLogic : LogicAbstractAPI
        {
            public bool StepCalled { get; private set; }
            public bool StartCalled { get; private set; }

            private double _boardWidth;
            private double _boardHeight;

            public override double BoardWidth => _boardWidth;
            public override double BoardHeight => _boardHeight;


            public override void StartSimulation(int count, double width, double height) => StartCalled = true;
            public override void Step() => StepCalled = true;
            public override IEnumerable<IBall> GetBalls() => new System.Collections.Generic.List<IBall>();
        }

        [TestMethod]
        public void StartSimulation_CallsLogicStart()
        {
            var testLogic = new TestLogic();
            var model = new PresentationModelAPI(testLogic);

            model.StartSimulation(5, 100, 100);

            Assert.IsTrue(testLogic.StartCalled);
        }

        [TestMethod]
        public void MoveBalls_CallsLogicStep()
        {
            var testLogic = new TestLogic();
            var model = new PresentationModelAPI(testLogic);

            model.MoveBalls();

            Assert.IsTrue(testLogic.StepCalled);
        }

        [TestMethod]
        public void GetBalls_ReturnsBallsFromLogic()
        {
            var testLogic = new TestLogic();
            var model = new PresentationModelAPI(testLogic);

            var balls = model.GetBalls();

            Assert.IsNotNull(balls);
            Assert.AreEqual(0, balls.Count());
        }
    }
}