using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.Linq;

namespace DataTest
{
    [TestClass]
    public class DataTests
    {

        [TestMethod]
        public void Ball_Constructor_SetsCorrectProperties()
        {

            double x = 10.5;
            double y = 20.5;
            double diameter = 15.0;

           
            IBall ball = new Ball(x, y, diameter);

           
            Assert.AreEqual(x, ball.Position.X);
            Assert.AreEqual(y, ball.Position.Y);
            Assert.AreEqual(diameter, ball.Diameter);
        }

        [TestMethod]
        public void Ball_Move_UpdatesPosition()
        {
            
            var ball = new Ball(0, 0, 10);
            double newX = 50.0;
            double newY = 60.0;

            
            ball.Move(newX, newY);

         
            Assert.AreEqual(newX, ball.Position.X);
            Assert.AreEqual(newY, ball.Position.Y);
        }

        [TestMethod]
        public void Ball_InverseSpeed_InvertsVelocityX()
        {
           
            var ball = new Ball(0, 0, 10);
            double initialVelocityX = ball.Position.VelocityX;

           
            ball.InverseSpeed("x");

            
            Assert.AreEqual(-initialVelocityX, ball.Position.VelocityX);
        }

        [TestMethod]
        public void DataAPI_CreateBalls_CreatesCorrectAmount()
        {
            
            DataAbstractAPI api = new DataAPI();
            int countToCreate = 5;

           
            api.CreateBalls(countToCreate, 100, 100);
            var balls = api.GetBalls().ToList();

           
            Assert.AreEqual(countToCreate, balls.Count);
        }

        [TestMethod]
        public void Ball_OnPropertyChanged_IsFiredOnMove()
        {
           
            var ball = new Ball(0, 0, 10);
            bool wasFired = false;
            ball.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Position")
                {
                    wasFired = true;
                }
            };

            
            ball.Move(10, 10);

            
            Assert.IsTrue(wasFired);
        }
    }
}