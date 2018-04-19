using System;
using System.Collections.Generic;

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

        private delegate double SetValue(double value);
        private Dictionary<string, SetValue> _methodsCheck;

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

            _methodsCheck = new Dictionary<string, SetValue>
            {
                {nameof(this.OuterHeight),
                    (double value) => this.OuterHeight = value },
                {nameof(this.OuterWidth),
                    (double value) => this.OuterWidth = value },
                {nameof(this.OuterLength),
                    (double value) => this.OuterLength = value },
                {nameof(this.InnerHeight),
                    (double value) => this.InnerHeight = value },
                {nameof(this.Interval),
                    (double value) => this.Interval = value },
            };
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


        private Dictionary<string, bool> _isValidParams =
            new Dictionary<string, bool>();

        public string ValidateParameter(string paramName, double value)
        {
            var result = String.Empty;

            if (_methodsCheck.ContainsKey(paramName))
            {
                try
                {
                    _methodsCheck[paramName](value);
                }
                catch (Exception ex)
                    when (ex is ArgumentException
                        || ex is ArgumentOutOfRangeException)
                {
                    result = ex.Message;
                }
                if (_isValidParams.ContainsKey(paramName) == false)
                {
                    _isValidParams.Add(paramName, true);
                }
                _isValidParams[paramName] =
                    (result == String.Empty) ? true : false;
            }

            return result;
        }

        public bool IsValid
        {
            get
            {
                var valid = true;
                try
                {
                    var iLength = InnerLength;
                    var iWidth = InnerWidth;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    valid = false;
                    return valid;
                }

                foreach (var p in _isValidParams)
                {
                    valid = p.Value;
                    if (valid == false) break;
                }
                return valid;
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
