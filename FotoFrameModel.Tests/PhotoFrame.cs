﻿using NUnit.Framework;
using FotoFrameModel;
using System.Collections.Generic;
using System;

namespace FotoFrameModel.Tests
{
    [TestFixture]
    public class PhotoFrameTests
    {
        private const double _minHeight = 1.0f;
        private const double _minForLengthAndWidth = 10.0f;
        private const double _max = 100.0f;
        private const double _maxInterval = 6.0f;
        private const double _value = 5.0f;
        private const double _smallest = _minHeight - 1000.0f;
        private const double _biggest = _max + 1000.0f;
        private const double _norm = _minForLengthAndWidth;

        private IPhotoFrame _photoFrame;

        [SetUp]
        public void Setup()
        {
            _photoFrame = GeneratePhoto();
        }

        /// <summary>
        /// Инициализирует параметры экземпляра класса PhotoFrame.
        /// </summary>
        /// <returns>Возвращает экземпляр класса PhotoFrame.</returns>
        public static PhotoFrame GeneratePhoto()
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

            return new PhotoFrame(outerWidth,
                outerHeight,
                outerLength,
                innerHeight,
                interval);
        }

        [Test(Description = "PhotoFrame Constructor Test" +
            " on requirements domain model")]
        public void PhotoFrameConstructorTest()
        {
            var innerWidth = CalcInnerWidth(_photoFrame.OuterWidth,
                _photoFrame.Interval);
            var innerLength = CalcInnerLength(_photoFrame.OuterLength,
                _photoFrame.Interval);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_max, _photoFrame.OuterWidth);
                Assert.AreEqual(_value, _photoFrame.OuterHeight);
                Assert.AreEqual(_max, _photoFrame.OuterLength);
                Assert.AreEqual(_value, _photoFrame.InnerHeight);
                Assert.AreEqual(_value, _photoFrame.Interval);
                Assert.AreEqual(innerLength, _photoFrame.InnerLength);
                Assert.AreEqual(innerWidth, _photoFrame.InnerWidth);
            });
        }

        /// <summary>
        /// Посчитать внутреннюю ширину фоторамки.
        /// </summary>
        /// <param name="outerWidth">Внешняя ширина фоторамки.</param>
        /// <param name="interval">Расстояние между
        ///     внешней и внутренней шириной фоторамки.</param>
        /// <returns>Возвращает значение внутренней ширины фоторамки.</returns>
        private double CalcInnerWidth(double outerWidth, double interval)
        {
            return outerWidth - 2 * interval;
        }

        /// <summary>
        /// Посчитать внутреннюю длину фоторамки.
        /// </summary>
        /// <param name="outerWidth">Внешняя длина фоторамки.</param>
        /// <param name="interval">Расстояние между
        ///     внешней и внутренней длиной фоторамки.</param>
        /// <returns>Возвращает значение внутренней длины фоторамки.</returns>
        private double CalcInnerLength(double outerLength, double interval)
        {
            return CalcInnerWidth(outerLength, interval);
        }

        [Test(Description = "Inner length property test positive")]
        [TestCase(_max, (_minHeight + _maxInterval) / 2.0f,
            TestName = "Outer length > inner length," +
                " interval = average value")]
        [TestCase(_max, _minHeight,
            TestName = "Outer length > inner length," +
                " interval = MinValue")]
        [TestCase(_max, _maxInterval,
            TestName = "Outer length > inner length," +
                " outerLength = max, interval = max")]
        [TestCase(_minForLengthAndWidth, _minHeight,
            TestName = "Outer length > inner length," +
                " outerLength = min, interval = min")]
        [TestCase(_minForLengthAndWidth, (_minHeight + _maxInterval) / 2.0f,
            TestName = "Outer length > inner length," +
                " outerLength = min, interval = average value")]
        [TestCase((_minForLengthAndWidth + _max) / 2.0f,
                (_minHeight + _maxInterval) / 2.0f,
                    TestName = "Outer length > inner length," +
                        "interval, outerLenght " +
                            "= average value")]
        public void InnerLengthTestPositive(double outerLength,
            double interval)
        {
            _photoFrame.Interval = interval;
            _photoFrame.OuterLength = outerLength;

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
            TestName = "Catch exception out of range outer length")]
        public void InnerLengthTestNegative(double outerLength,
            double interval)
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _photoFrame.OuterLength = outerLength;
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _photoFrame.Interval = interval;
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    var t = _photoFrame.InnerLength;
                });
            });

        }

        [Test(Description = "Inner width property test positive")]
        [TestCase(_max, (_minHeight + _maxInterval) / 2.0f,
            TestName = "Outer width > inner width," +
                " interval = average value")]
        [TestCase(_max, _minHeight,
            TestName = "Outer width > inner width," +
                " interval = MinValue")]
        [TestCase(_max, _maxInterval,
            TestName = "Outer width > inner width," +
                " outerWidth = max, interval = max")]
        [TestCase(_minForLengthAndWidth, _minHeight,
            TestName = "Outer width > inner width," +
                " outerWidth = min, interval = min")]
        [TestCase(_minForLengthAndWidth, (_minHeight + _maxInterval) / 2.0f,
            TestName = "Outer width > inner width," +
                " outerWidth = min, interval = average value")]
        [TestCase((_minForLengthAndWidth + _max) / 2.0f,
                (_minHeight + _maxInterval) / 2.0f,
                    TestName = "Outer width > inner width," +
                        "interval, outerWidth = average value")]
        public void InnerWidthTestPositive(double outerWidth,
            double interval)
        {
            _photoFrame.Interval = interval;
            _photoFrame.OuterWidth = outerWidth;

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
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _photoFrame.OuterWidth = outerWidth;
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _photoFrame.Interval = interval;
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    var t = _photoFrame.InnerWidth;
                });
            });
        }

        public delegate double SetValue(double value);

        [Test(Description = "Check all steps of validation parameters" +
            "photo frame negative")]
        [TestCase(0, 0, 0, 0, 0, false,
            TestName = "Wrong params")]
        [TestCase(_smallest, _norm, 1, _norm, _norm, false,
            TestName = "Outer width < min")]
        [TestCase(_biggest, _norm, 1, _norm, _norm, false,
            TestName = "Outer width > max")]
        [TestCase(_norm, _smallest, 1, _norm, _norm, false,
            TestName = "Outer length < min")]
        [TestCase(_norm, _biggest, 1, _norm, _norm, false,
            TestName = "Outer length > max")]
        [TestCase(_norm, _norm, 1, _smallest, _norm, false,
            TestName = "Outer height < min")]
        [TestCase(_norm, _norm, 1, _biggest, _norm, false,
            TestName = "Outer height > max")]
        [TestCase(_norm, _norm, _smallest, _norm, _norm, false,
            TestName = "Interval < min")]
        [TestCase(_norm, _norm, _biggest, _norm, _norm, false,
            TestName = "Interval > max")]
        [TestCase(_norm, _norm, 1, _norm, _smallest, false,
            TestName = "Inner height < min")]
        [TestCase(_norm, _norm, 1, _norm, _biggest, false,
            TestName = "Inner height > max")]
        [TestCase(_norm, _max, _maxInterval, _norm, _norm, false,
            TestName = "Inner width is wrong," +
                "outer width less than 2x interval")]
        [TestCase(_max, _norm, _maxInterval, _norm, _norm, false,
            TestName = "Inner length is wrong," +
                " outer length less than 2x interval")]
        [TestCase(_norm, _norm, 1, _norm, _norm + 10.0f, false,
            TestName = "Outer height < inner height")]
        public void CheckParamsTestNegative(
            double outerWidth, double outerLength, double interval,
                double outerHeight, double innerHeight, bool expected)
        {
            Assert.Multiple(() =>
            {
                CheckParamsTestPositive(outerWidth, outerLength,
                        interval, outerHeight, innerHeight, expected);
            });
        }

        [Test(Description = "Check params photo frame test positive")]
        [TestCase(_norm + 2, _norm + 2, 1, _norm, _norm, true,
            TestName = "Valid params")]
        public void CheckParamsTestPositive(
            double outerWidth, double outerLength, double interval,
                double outerHeight, double innerHeight, bool expected)
        {
            var frameParams = GetListParams(outerWidth, outerLength,
                interval, outerHeight, innerHeight);

            foreach (var p in frameParams)
            {
                try
                {
                    p.Item3(p.Item2);
                }
                catch (ArgumentException)
                { }
            }

            Assert.AreEqual(expected, _photoFrame.IsValid);
        }

        [Test(Description = "Check checkable params - " +
            "substrate and stand of photo frame")]
        [TestCase(true, true, TestName = "Substrate and stand checked")]
        [TestCase(true, false, TestName = "Substrate checked")]
        [TestCase(false, true, TestName = "Stand checked")]
        [TestCase(false, false, TestName = "Substrate and stand is " +
            "not checked")]
        public void CheckCheckedParams(bool substrate, bool stand)
        {
            _photoFrame.HasSubstrate = substrate;
            _photoFrame.HasStand = stand;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(substrate, _photoFrame.HasSubstrate);
                Assert.AreEqual(stand, _photoFrame.HasStand);
            });
        }

        /// <summary>
        /// Упаковать параметры фоторамки в список пар в виде
        ///     (название параметра, свойство параметра)
        /// </summary>
        /// <param name="outerWidth">Внешняя ширина фоторамки.</param>
        /// <param name="outerLength">Внешняя длина фоторамки.</param>
        /// <param name="interval">Расстояние между внешними и внутренними
        ///     параметрами(шириной и длиной) фоторамки.</param>
        /// <param name="outerHeight">Внешняя высота фоторамки.</param>
        /// <param name="innerHeight">Внутрення высота фоторамки.</param>
        /// <returns>Возвращает список пар в виде
        ///     (название параметра, свойство параметра)</returns>
        private List<Tuple<string, double, SetValue>> GetListParams(double outerWidth,
            double outerLength, double interval, double outerHeight,
                double innerHeight)
        {
            return new List<Tuple<string, double, SetValue>>
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
        }
    }
}
