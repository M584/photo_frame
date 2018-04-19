﻿using System;

namespace FotoFrameModel
{
    public interface IChecker
    {
        bool IsValid { get; }
    }


    public interface IPhotoFrame
    {
        double OuterWidth { get; set; }
        double OuterHeight { get; set; }
        double OuterLength { get; set; }
        double InnerHeight { get; set; }
        double Interval { get; set; }
        double InnerWidth { get; }
        double InnerLength { get; }
    }


    public class PhotoFrameTemplate : IPhotoFrame, IChecker
    {
        private BorderConditions _outerWidth;
        private BorderConditions _outerHeight;
        private BorderConditions _outerLength;
        private BorderConditions _innerHeight;
        private BorderConditions _interval;

        public PhotoFrameTemplate(
            BorderConditions outerWidth,
            BorderConditions outerHeight,
            BorderConditions outerLength,
            BorderConditions innerHeight,
            BorderConditions interval)
        {
            _outerWidth = outerWidth;
            _outerHeight = outerHeight;
            _outerLength = outerLength;
            _innerHeight = innerHeight;
            _interval = interval;
        }

        public double OuterWidth
        {
            get
            {
                return _outerWidth.Value;
            }
            set
            {
                _outerWidth.Value = value;
            }
        }
        public double OuterHeight
        {
            get
            {
                return _outerHeight.Value;
            }
            set
            {
                _outerHeight.Value = value;
            }
        }
        public double OuterLength
        {
            get
            {
                return _outerLength.Value;
            }
            set
            {
                _outerLength.Value = value;
            }
        }
        public double InnerHeight
        {
            get
            {
                return _innerHeight.Value;
            }
            set
            {
                _innerHeight.Value = value;
            }
        }
        public double Interval
        {
            get
            {
                return _interval.Value;
            }
            set
            {
                _interval.Value = value;
            }
        }


        /// <summary>
        /// Вычисляет значение внутренних параметров:
        ///     ширины или длины
        /// </summary>
        /// <param name="outerParam">Значение внешней ширины 
        ///     или длины</param>
        /// <param name="outerParamName">Имя внешнего параметра
        ///     (использовать nameof для получения имени)</param>
        /// <param name="outerParamLabel">Название внешнего параметра
        ///     в родительном падеже</param>
        /// <returns>Посчитанный внутренний параметр:
        ///     ширина или длина</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Возникает, если нарушена зависимость между
        ///     внутренним и внешним параметром</exception>
        private double CalcInnerParam(double outerParam,
            string outerParamName, string outerParamLabel)
        {
            var innerParam = outerParam - Interval * 2;

            if (!(innerParam > 0) || !(outerParam > innerParam))
            {
                var msg = $"Значение внешней {outerParamLabel}" +
                    $" должно быть больше " +
                    $"двойного интервала = {Interval * 2}" +
                    $"({Interval}*2)";
                throw new ArgumentOutOfRangeException(
                    outerParamName, outerParam, msg);
            }

            return innerParam;
        }

        public double InnerWidth
        {
            get
            {
                return CalcInnerParam(OuterWidth,
                    nameof(OuterWidth), "ширины");
            }
        }

        public double InnerLength
        {
            get
            {
                return CalcInnerParam(OuterLength,
                    nameof(OuterLength), "высоты");
            }
        }

        public bool IsValid => throw new NotImplementedException();
    }

    /// <summary>
    /// Диапазон допустимых значений параметра
    /// </summary>
    public class BorderConditions
    {
        private double _minimum = 0.0f;
        private double _maximum = 0.0f;
        private double _value = 0.0f;

        /// <summary>
        /// Установление граничных условий для значения
        /// </summary>
        /// <param name="min">Минимальное значение</param>
        /// <param name="value">Текущее значение</param>
        /// <param name="max">Максимальное значение</param>
        public BorderConditions(double min, double value, double max)
        {
            if (min > max)
            {
                var msg = $"Минимальное значение = {min} " +
                    $"больше максимального = {max}";
                throw new ArgumentException(msg, nameof(min));
            }

            Min = min;
            Max = max;
            Value = value;
        }

        /// <summary>
        /// Максимальное значение диапазона
        /// </summary>
        public double Max
        {
            get => _maximum;
            private set
            {
                _maximum = value;
            }
        }

        /// <summary>
        /// Минимальное значение диапазона
        /// </summary>
        public double Min
        {
            get => _minimum;
            private set
            {
                _minimum = value;
            }
        }

        /// <summary>
        /// Значение, входящее в диапазон
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                if (value > Max)
                {
                    var msg = $"Заданное значение = {value} больше," +
                        $" чем максимальное значение = {Max}";
                    throw new ArgumentException(msg, nameof(Value));
                }

                if (value < Min)
                {
                    var msg = $"Заданное значение = {value} меньше," +
                        $" чем минимальное значение = {Min}";
                    throw new ArgumentException(msg, nameof(Value));
                }

                _value = value;
            }
        }
    }
}
