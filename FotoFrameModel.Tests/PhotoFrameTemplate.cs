using NUnit.Framework;
using FotoFrameModel;
using System;

namespace FotoFrameModel.Tests
{
    [TestFixture]
    public class PhotoFrameTemplateTests
    {
        private const double _minHeight = 1.0f;
        private const double _minForLengthAndWidth = 10.0f;
        private const double _max = 100.0f;
        private const double _maxInterval = 6.0f;
        private const double _value = 5.0f;

        private PhotoFrameTemplate _photoFrame;

        [SetUp]
        public void Setup()
        {
            var outerWidth = new BorderConditions(_minForLengthAndWidth, _minForLengthAndWidth, _max);
            var outerHeight = new BorderConditions(_minHeight, _value, _minForLengthAndWidth);
            var outerLength = new BorderConditions(_minForLengthAndWidth, _minForLengthAndWidth, _max);
            var innerHeight = new BorderConditions(_minHeight, _value, _minForLengthAndWidth);
            var interval = new BorderConditions(_minHeight, _value, _maxInterval);

            _photoFrame = new PhotoFrameTemplate(outerWidth,
                outerHeight,
                outerLength,
                innerHeight,
                interval);
        }

        [Test(Description = "PhotoFrameTemlate Constructor Test on requirements domain model")]
        public void PhotoFrameTemplateConstructorTest()
        {
            var template = _photoFrame;

            var innerWidth = CalcInnerWidth(template.OuterWidth, template.Interval);
            var innerLength = CalcInnerLength(template.OuterLength, template.Interval);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_minForLengthAndWidth, template.OuterWidth);
                Assert.AreEqual(_value, template.OuterHeight);
                Assert.AreEqual(_minForLengthAndWidth, template.OuterLength);
                Assert.AreEqual(_value, template.InnerHeight);
                Assert.AreEqual(_value, template.Interval);
                Assert.AreEqual(innerLength, template.InnerLength);
                Assert.AreEqual(innerWidth, template.InnerWidth);
            });
        }

        private double CalcInnerWidth(double outerWidth, double interval)
        {
            return outerWidth - 2 * interval;
        }

        private double CalcInnerLength(double outerLength, double interval)
        {
            return CalcInnerWidth(outerLength, interval);
        }

        [Test(Description = "Inner length less than outer length")]
        [TestCase(20, _maxInterval)]
        [TestCase(15, _minHeight)]
        public void InnerLengthLessOuterLength(double outerLength, double interval)
        {
            _photoFrame.OuterLength = outerLength;
            _photoFrame.Interval = interval;

            var expectedInnerLength = CalcInnerLength(outerLength, interval);

            Assert.AreEqual(expectedInnerLength, _photoFrame.InnerLength);
        }
    }
}
