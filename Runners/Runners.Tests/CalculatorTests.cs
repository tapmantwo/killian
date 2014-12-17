using NUnit.Framework;

namespace Runners.Tests
{
    public class CalculatorTests
    {
        [Test]
        public void InitiallyZero()
        {
            Scenario<Calculator>.
                With(new Calculator()).
                Then(c => Assert.That(c.Total, Is.EqualTo(0))).
                Run();
        }

        [Test]
        public void ClearWorks()
        {
            Scenario<Calculator>.
                With(new Calculator()).
                Given(c => c.Total = 10).
                When(c => c.Clear()).
                Then(c => Assert.That(c.Total, Is.EqualTo(0))).
                Run();
        }

        [Test]
        public void AddingWorks()
        {
            Scenario<Calculator>.
                With(new Calculator()).
                Given(c => c.Total = 1).
                When(c => c.AddValue(2)).
                Then(c => Assert.That(c.Total, Is.EqualTo(3))).
                Run();
        }

        [Test]
        public void RepeatedAddingWorks()
        {
            Scenario<Calculator>.
                With(new Calculator()).
                Given(c => c.Total = 0).
                When(c => c.AddValue(1)).
                Happens(Times.Many(10)).
                Then(c => Assert.That(c.Total, Is.EqualTo(10))).
                Run();
        }

        [Test]
        public void RepeatedSubtractingWorks()
        {
            Scenario<Calculator>.
                With(new Calculator()).
                Given(c => c.Total = 100).
                When(c => c.SubtractValue(10)).
                Happens(Times.Many(5)).
                Then(c => Assert.That(c.Total, Is.EqualTo(50))).
                Run();
        }

        [Test]
        public void SubtractingWorks()
        {
            Scenario<Calculator>.
                With(new Calculator()).
                Given(c => c.Total = 10).
                When(c => c.SubtractValue(2)).
                Then(c => Assert.That(c.Total, Is.EqualTo(8))).
                Run();
        }
    }
}