using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationViewModel;
using PresentationModel;
using Data;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class BoardViewModelTests
{
    private class TestModel : PresentationModelAbstractAPI
    {
        public bool SimulationStarted { get; set; }
        public override void StartSimulation(int count, double w, double h) => SimulationStarted = true;
        public override void MoveBalls() { }
        public override IEnumerable<IBall> GetBalls() => new List<IBall> { new TestBall(), new TestBall() };
    }

    private class TestBall : IBall
    {
        public IVector Position => null;
        public double Diameter => 20;
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public void Move(double x, double y) { }
        public void InverseSpeed(string v) { }
    }

    [TestMethod]
    public void Constructor_InitializesBallsCollection()
    {
        // Arrange
        var testModel = new TestModel();

        // Act
        var viewModel = new BoardViewModel(testModel, 2, 400, 400);

        // Assert
        Assert.IsTrue(testModel.SimulationStarted);
        Assert.AreEqual(2, viewModel.Balls.Count);
    }
}