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

        [Test(Description = "PhotoFrameTemlate Constructor Test on requirements domain model")]
        public void PhotoFrameTemplateConstructorTest()
        {
            var outerWidth = new BorderConditions(_minForLengthAndWidth, _minForLengthAndWidth, _max);
            var outerHeight = new BorderConditions(_minHeight, _value, _minForLengthAndWidth);
            var outerLength = new BorderConditions(_minForLengthAndWidth, _minForLengthAndWidth, _max);
            var innerHeight = new BorderConditions(_minHeight, _value, _minForLengthAndWidth);
            var interval = new BorderConditions(_minHeight, _value, _maxInterval);

            var template = new PhotoFrameTemplate(outerWidth,
                outerHeight,
                outerLength,
                innerHeight,
                interval);

            var innerWidth = template.OuterWidth - template.Interval * 2;
            var innerLength = template.OuterLength - template.Interval * 2;

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

        public 
    }
}
