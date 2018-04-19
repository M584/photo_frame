using NUnit.Framework;
using FotoFrameModel;
using System.Collections.Generic;
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
            var outerWidth = new BorderConditions(_minForLengthAndWidth,
                _max, _max);
            var outerHeight = new BorderConditions(_minHeight,
                _value, _minForLengthAndWidth);
            var outerLength = new BorderConditions(_minForLengthAndWidth,
                _max, _max);
            var innerHeight = new BorderConditions(_minHeight,
                _value, _minForLengthAndWidth);
            var interval = new BorderConditions(_minHeight,
                _value, _maxInterval);

            _photoFrame = new PhotoFrameTemplate(outerWidth,
                outerHeight,
                outerLength,
                innerHeight,
                interval);
        }

        [Test(Description = "PhotoFrameTemlate Constructor Test" +
            " on requirements domain model")]
        public void PhotoFrameTemplateConstructorTest()
        {
            var template = _photoFrame;

            var innerWidth = CalcInnerWidth(template.OuterWidth,
                template.Interval);
            var innerLength = CalcInnerLength(template.OuterLength,
                template.Interval);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_max, template.OuterWidth);
                Assert.AreEqual(_value, template.OuterHeight);
                Assert.AreEqual(_max, template.OuterLength);
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

        [Test(Description = "Inner length property test positive")]
        [TestCase(_max, (_minHeight + _maxInterval) / 2.0f,
            TestName = "Inner length > outerLength," +
                " interval = average value")]
        [TestCase(_max, _minHeight,
            TestName = "Inner length > outerLength," +
                " interval = MinValue")]
        [TestCase(_minForLengthAndWidth, _minHeight,
            TestName = "Inner length > outerLength," +
                " outerLength = min, interval = min")]
        [TestCase(_minForLengthAndWidth, (_minHeight + _maxInterval) / 2.0f,
            TestName = "Inner length > outerLength," +
                " outerLength = min, interval = average value")]
        public void InnerLengthTestPositive(double outerLength,
            double interval)
        {
            _photoFrame.OuterLength = outerLength;
            _photoFrame.Interval = interval;

            var expectedInnerLength = CalcInnerLength(outerLength,
                interval);
            var cond = _photoFrame.OuterLength > _photoFrame.InnerLength;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedInnerLength,
                    _photoFrame.InnerLength);
                Assert.That(_photoFrame.InnerLength > 0);
                Assert.That(cond);
            });
        }

        [Test(Description = "Inner length property test negative")]
        [TestCase(_minForLengthAndWidth, _maxInterval,
            TestName = "Catch exception out of range inner length")]
        public void InnerLengthTestNegative(double outerLength,
            double interval)
        {
            _photoFrame.OuterLength = outerLength;
            _photoFrame.Interval = interval;

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var t = _photoFrame.InnerLength;
            });
        }

        [Test(Description = "Inner width property test positive")]
        [TestCase(_max, (_minHeight + _maxInterval) / 2.0f,
            TestName = "Inner width > outer width," +
                " interval = average value")]
        [TestCase(_max, _minHeight,
            TestName = "Inner width > outer width," +
                " interval = MinValue")]
        [TestCase(_minForLengthAndWidth, _minHeight,
            TestName = "Inner width > outer width," +
                " outerLength = min, interval = min")]
        [TestCase(_minForLengthAndWidth, (_minHeight + _maxInterval) / 2.0f,
            TestName = "Inner width > outer width," +
                " outerWidth = min, interval = average value")]
        public void InneWidthTestPositive(double outerWidth,
            double interval)
        {
            _photoFrame.OuterWidth = outerWidth;
            _photoFrame.Interval = interval;

            var expectedInnerWidth = CalcInnerLength(outerWidth,
                interval);
            var cond = _photoFrame.OuterWidth > _photoFrame.InnerWidth;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedInnerWidth,
                    _photoFrame.InnerWidth);
                Assert.That(_photoFrame.InnerWidth > 0);
                Assert.That(cond);
            });
        }

        [Test(Description = "Inner width property test negative")]
        [TestCase(_minForLengthAndWidth, _maxInterval,
            TestName = "Catch exception out of range inner width")]
        public void InnerWidthTestNegative(double outerWidth,
            double interval)
        {
            _photoFrame.OuterWidth = outerWidth;
            _photoFrame.Interval = interval;

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var t = _photoFrame.InnerWidth;
            });
        }

        public delegate double SetValue(double value);

        [Test(Description = "Validation photo frame template test")]
        [TestCase(10, 10, 1, 10, 10, true, TestName = "Valid params")]
        [TestCase(0, 0, 0, 0, 0, false, TestName = "Wrong interval")]
        [TestCase(-10, 10, 1, 10, 10, false, TestName = "Wrong outer width")]
        [TestCase(10, -10, 1, 10, 10, false, TestName = "Wrong outer length")]
        [TestCase(10, 10, 1, 0, 10, false, TestName = "Wrong outer height")]
        [TestCase(10, 10, 1, 10, 0, false, TestName = "Wrong inner height")]
        public void IsValidAndValidateParameterTest(double outerWidth,
            double outerLength, double interval, double outerHeight,
                double innerHeight, bool expected)
        {
            var frameParams = new List<Tuple<string, double, SetValue>>
            {
                new Tuple<string, double, SetValue>(
                    nameof(_photoFrame.OuterWidth), outerWidth,
                        ((double value) => _photoFrame.OuterWidth = value)),
                new Tuple<string, double, SetValue>(
                    nameof(_photoFrame.OuterLength), outerLength,
                        ((double value) => _photoFrame.OuterLength = value)),
                new Tuple<string, double, SetValue>(
                    nameof(_photoFrame.Interval), interval,
                        ((double value) => _photoFrame.Interval = value)),
                new Tuple<string, double, SetValue>(
                    nameof(_photoFrame.OuterHeight), outerHeight,
                        ((double value) => _photoFrame.OuterHeight = value)),
                new Tuple<string, double, SetValue>(
                    nameof(_photoFrame.InnerHeight), innerHeight,
                        ((double value) => _photoFrame.InnerHeight = value))
            };

            foreach (var p in frameParams)
            {
                try
                {
                    _photoFrame.ValidateParameter(p.Item1, p.Item2);
                    p.Item3(p.Item2);
                }
                catch (Exception ex)
                   when (ex is ArgumentException
                       || ex is ArgumentOutOfRangeException)
                { }
            }
            Assert.AreEqual(expected, _photoFrame.IsValid);
        }
    }
}
