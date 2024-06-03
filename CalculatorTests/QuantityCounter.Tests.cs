using NUnit.Framework;
using System.Collections.Generic;
using Calculator;

namespace Calculator.Tests
{
    [TestFixture]
    public class QuantityCounterTests
    {

        [Test]
        public void TestGetSingleType_WhenDistanceIsOne_ShouldReturnDistance()
        {
            var counter = new QuantityCounter { DistanceQuantNum = 1 };
            Assert.That(counter.GetSingleType(), Is.EqualTo(ValueType.Distance));
        }

        [Test]
        public void TestMultiplicationOperator()
        {
            var counter1 = new QuantityCounter { DistanceQuantNum = 1, TimeQuantNum = 2, SpeedQuantNum = 1, Scalar = 25.4};
            var counter2 = new QuantityCounter { DistanceQuantNum = 2, TimeQuantNum = 1, SpeedQuantNum = 1, Scalar = 10};
            var result = counter1 * counter2;

            Assert.That(result != null);
            Assert.That(result.DistanceQuantNum, Is.EqualTo(3));
            Assert.That(result.TimeQuantNum, Is.EqualTo(3));
            Assert.That(result.SpeedQuantNum, Is.EqualTo(2));
            Assert.That(result.Scalar, Is.EqualTo(254));
        }

        [Test]
        public void TestDivisionOperator()
        {
            var counter1 = new QuantityCounter { DistanceQuantNum = 1, TimeQuantNum = 2, SpeedQuantNum = 1, Scalar = 25.4 };
            var counter2 = new QuantityCounter { DistanceQuantNum = 2, TimeQuantNum = 1, SpeedQuantNum = 1, Scalar = 10 };
            var result = counter1 / counter2;

            Assert.That(result != null);
            Assert.That(result.DistanceQuantNum, Is.EqualTo(-1));
            Assert.That(result.TimeQuantNum, Is.EqualTo(1));
            Assert.That(result.SpeedQuantNum, Is.EqualTo(0));
            Assert.That(result.Scalar, Is.EqualTo(2.54));
        }

        [Test]
        public void TestAdditionOperator()
        {
            var counter1 = new QuantityCounter { DistanceQuantNum = 1, TimeQuantNum = 2, SpeedQuantNum = 1, Scalar = 25.4 };
            var counter2 = new QuantityCounter { DistanceQuantNum = 1, TimeQuantNum = 2, SpeedQuantNum = 1, Scalar = 10 };
            var result = counter1 + counter2;

            Assert.That(result != null);
            Assert.That(result.Scalar, Is.EqualTo(35.4));
        }

        [Test]
        public void TestSimplify()
        {
            var counter1 = new QuantityCounter { DistanceQuantNum = 0, TimeQuantNum = 1, SpeedQuantNum = 1, Scalar = 25.4 };
            Assert.That(counter1.Simplify().GetSingleType(), Is.EqualTo(ValueType.Distance));
        }
    }
}
