using NUnit.Framework;
using FotoFrameModel;

namespace Tests
{
    [TestFixture]
    public class BorderConditionsTests
    {
        private BorderConditions _parameter;

        private const double _min = -10.0f;
        private const double _value = 0.0f;
        private const double _max = 10.0f;

        [SetUp]
        public void Setup()
        {
            _parameter = new BorderConditions(_min, _value, _max);
        }

        [Test(Description = "BorderConditions constructor test")]
        [TestCase(_min, _value, _max)]
        [TestCase(_min + 10, _value + 10, _max + 10)]
        public void BorderConditionsConstructorTest(double min, double value, double max)
        {
            var parameter = new BorderConditions(min, value, max);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(min, parameter.Min);
                Assert.AreEqual(value, parameter.Value);
                Assert.AreEqual(max, parameter.Max);
            });
        }


    }
}