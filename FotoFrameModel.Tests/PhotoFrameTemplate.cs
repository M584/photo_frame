using NUnit.Framework;
using FotoFrameModel;
using System;

namespace FotoFrameModel.Tests
{
    [TestFixture]
    public class PhotoFrameTemplateTests
    {
        private const double _min = 1.0f;
        private const double _max = 100.0f;
        private const double _maxHeight = 6.0f;
        private const double _value = 5.0f;

        [Test(Description = "PhotoFrameTemlate Constructor Test positive")]
        public void PhotoFrameTemplateConstructorTest()
        {
            var outerWidth = new BorderConditions(_min, _value, _max);
            var outerHeight = new BorderConditions(_min, _value, _max / 10.0f);
            var outerLength = new BorderConditions(_min, _value, _max);
            var innerHeight = new BorderConditions(_min, _value, _max / 10.0f);
            var interval = new BorderConditions(_min, _value, _maxHeight);

            var template = new PhotoFrameTemplate(outerWidth,
                outerHeight,
                outerLength,
                innerHeight,
                interval);

            var innerWidth = template.OuterWidth - template.Interval * 2;
            var innerLength = template.OuterLength - template.Interval * 2;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_value, template.OuterWidth);
                Assert.AreEqual(_value, template.OuterHeight);
                Assert.AreEqual(_value, template.OuterLength);
                Assert.AreEqual(_value, template.InnerHeight);
                Assert.AreEqual(_value, template.Interval);
                Assert.AreEqual(innerLength, template.InnerLength);
                Assert.AreEqual(innerWidth, template.InnerWidth);
            });
        }
    }
}
