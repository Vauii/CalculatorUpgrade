using Calculator;
using NUnit.Framework;
using System.Collections.Generic;

namespace Calculator.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            Calculator.values = new Dictionary<string, (ValueType type, double value)>
            {
                {"0", (ValueType.Scalar, 0)},
                {"1", (ValueType.Scalar, 1)},
                {"A", (ValueType.Distance, 2.2)},
                {"B", (ValueType.Distance, 4.2)},
                {"C", (ValueType.Speed, 3.5)},
                {"D", (ValueType.Time, 10)},
                {"E", (ValueType.Scalar, 100)},
                {"F", (ValueType.Speed, 1)},
                {"G", (ValueType.Time, 1)}
                
            };
        }

        [Test]
        public void TestAdditionException()
        {
            var expression = new List<string> { "A", "+", "C" };
            Assert.Throws<Exception>(() => Calculator.AnalyzeExpressionType(expression));
        }

        [Test]
        public void TestSubtractionException()
        {
            var expression = new List<string> { "A", "-", "C" };
            Assert.Throws<Exception>(() => Calculator.AnalyzeExpressionType(expression));
        }

        [Test]
        public void TestAdditionType()
        {
            var expression = new List<string> { "A", "+", "B" };
            var result = Calculator.AnalyzeExpressionType(expression);
            Assert.That(result, Is.EqualTo(Calculator.values["A"].type));
        }

        [Test]
        public void TestSubtractionType()
        {
            var expression = new List<string> { "A", "-", "B" };
            var result = Calculator.AnalyzeExpressionType(expression);
            Assert.That(result, Is.EqualTo(Calculator.values["A"].type));
        }


        [Test]
        public void TestAdditionValue()
        {
            var expression = new List<string> { "A", "+", "B" };
            var result = Calculator.AnalyzeExpressionValue(expression);
            Assert.That(result, Is.EqualTo(Calculator.values["A"].value + Calculator.values["B"].value));
        }

        [Test]
        public void TestSubtractionValue()
        {
            var expression = new List<string> { "A", "-", "B" };
            var result = Calculator.AnalyzeExpressionValue(expression);
            Assert.That(result, Is.EqualTo(Calculator.values["A"].value - Calculator.values["B"].value));
        }

        [Test]
        public void TestAdditionValueWithZero()
        {
            var expression = new List<string> { "A", "+", "0" };
            var result = Calculator.AnalyzeExpressionValue(expression);
            Assert.That(result, Is.EqualTo(Calculator.values["A"].value));
        }


        [Test]
        public void TestMultiplicationException()
        {
            var expression = new List<string> { "A", "*", "B" };
            Calculator.expression = expression;
            Assert.Throws<Exception>(() => Calculator.Compute());
        }

        [Test]
        public void TestDivisionException()
        {
            var expression = new List<string> { "A", "/", "0" };
            Assert.Throws<Exception>(() => Calculator.AnalyzeExpressionValue(expression));
        }

        [Test]
        public void TestMultiplicatioValue()
        {
            var expression = new List<string> { "D", "*", "C" };
            var result = Calculator.AnalyzeExpressionValue(expression);
            Assert.That(result, Is.EqualTo(Calculator.values["D"].value * Calculator.values["C"].value));
        }

        [Test]
        public void TestMultiplicatioType()
        {
            var expression = new List<string> { "D", "*", "C" };
            var result = Calculator.AnalyzeExpressionType(expression);
            Assert.That(result, Is.EqualTo(ValueType.Distance));
        }

        [Test]
        public void TestAnalyzingType()
        {
            var expression = new List<string> { "D", "/", "(", "A", "/", "C", ")" };
            var result = Calculator.AnalyzeExpressionType(expression);
            Assert.That(result, Is.EqualTo(ValueType.Scalar));
        }


        [Test]
        public void TestAnalyzingTypeWithDuplicatesByType()
        {
            var expression = new List<string> { "F", "*", "(", "*", "G", "*", "D", "/", "B"};
            var result = Calculator.AnalyzeExpressionType(expression);
            Assert.That(result, Is.EqualTo(ValueType.Distance));
        }

        [Test]
        public void TestRemoveValue()
        {
            var expression = new List<string> { "D", "/", "(", "A", "/", "C", ")" };
            Calculator.expression = expression;
            Calculator.RemoveValue("remove value D");
            var result = Calculator.AnalyzeExpressionValue(expression);
            Assert.That(result, Is.EqualTo(0));
        }
    }
}

