using System;
using System.Collections.Generic;

namespace FotoFrameModel
{
    /// <summary>
    /// Интерфейс для проверки параметров фоторамки 
    ///     на требования предметной области.
    /// </summary>
    public interface IChecker
    {
        /// <summary>
        /// Проверяет удовлетворяют ли параметры фоторамки
        ///     требованиям предметной области.
        /// </summary>
        bool IsValid { get; }
    }

    /// <summary>
    /// Интерфейс параметров фоторамки.
    /// </summary>
    public interface IPhotoFrame
    {
        /// <summary>
        /// Внешняя ширина фоторамки.
        /// </summary>
        double OuterWidth { get; set; }

        /// <summary>
        /// Внешняя высота фоторамки.
        /// </summary>
        double OuterHeight { get; set; }

        /// <summary>
        /// Внешняя длина фоторамки.
        /// </summary>
        double OuterLength { get; set; }

        /// <summary>
        /// Внутренняя высота фоторамки.
        /// </summary>
        double InnerHeight { get; set; }

        /// <summary>
        /// Расстояние между внутренней и внешней частями 
        ///     (кроме, высоты) фоторамки.
        /// </summary>
        double Interval { get; set; }

        /// <summary>
        /// Внутренняя ширина фоторамки.
        /// </summary>
        double InnerWidth { get; }

        /// <summary>
        /// Внутренняя длина фоторамки.
        /// </summary>
        double InnerLength { get; }
    }

    /// <summary>
    /// Шаблон фоторамки
    /// </summary>
    public class PhotoFrameTemplate : IPhotoFrame, IChecker
    {
        private BorderConditions _outerWidth;
        private BorderConditions _outerHeight;
        private BorderConditions _outerLength;
        private BorderConditions _innerHeight;
        private BorderConditions _interval;

        private delegate double SetValue(double value);
        /// <summary>
        /// Словарь с методами для проверки требований 
        ///     параметров фоторамки:
        ///     название параметра и его метод проверки.
        /// </summary>
        private Dictionary<string, SetValue> _methodsCheck;

        /// <summary>
        /// Установка граничных значений для параметров фоторамки.
        /// </summary>
        /// <param name="outerWidth">Граничные условия
        ///     для внешней ширины фоторамки.</param>
        /// <param name="outerHeight">Граничные условия
        ///     для внешней высоты фоторамки.</param>
        /// <param name="outerLength">Граничные условия
        ///     для внешней длины фоторамки.</param>
        /// <param name="innerHeight">Граничные условия
        ///     для внутренней высоты фоторамки.</param>
        /// <param name="interval">Граничные условия
        ///     для расстояния между внутренней и внешней частями
        ///     фоторамки.</param>
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

        /// <summary>
        /// Внешняя ширина фоторамки.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public double OuterWidth
        {
            get
            {
                return _outerWidth.Value;
            }
            set
            {
                try
                {
                    _outerWidth.Value = value;
                    var calcInnerWidth = InnerWidth;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[nameof(OuterWidth)] = false;
                    throw ex;
                }
                _isValidParams[nameof(OuterWidth)] = true;
            }
        }

        /// <summary>
        /// Внешняя высота фоторамки.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public double OuterHeight
        {
            get
            {
                return _outerHeight.Value;
            }
            set
            {
                try
                {
                    _outerHeight.Value = value;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[nameof(OuterHeight)] = false;
                    throw ex;
                }
                _isValidParams[nameof(OuterHeight)] = true;
            }
        }

        /// <summary>
        /// Внешняя длина фоторамки.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public double OuterLength
        {
            get
            {
                return _outerLength.Value;
            }
            set
            {      
                try
                {
                    _outerLength.Value = value;
                    var calcInnerLength = InnerLength;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[nameof(OuterLength)] = false;
                    throw ex;
                }
                _isValidParams[nameof(OuterLength)] = true;
            }
        }

        /// <summary>
        /// Внутренняя высота фоторамки.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public double InnerHeight
        {
            get
            {
                return _innerHeight.Value;
            }
            set
            {
                try
                {
                    _innerHeight.Value = value;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[nameof(InnerHeight)] = false;
                    throw ex;
                }
                _isValidParams[nameof(InnerHeight)] = true;
            }
        }

        /// <summary>
        /// Расстояние между внутренней и внешней частями 
        ///     (кроме высот) фоторамки.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public double Interval
        {
            get
            {
                return _interval.Value;
            }
            set
            {
                try
                {
                    _interval.Value = value;
                    var iLength = InnerLength;
                    var iWidth = InnerWidth;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[nameof(Interval)] = false;
                    throw ex;
                }
                _isValidParams[nameof(Interval)] = true;
            }
        }  

        /// <summary>
        /// Словарь с параметрами фоторамки: имя параметра и 
        ///     удовлетворяет ли он требованиям предметной области.
        /// </summary>
        private Dictionary<string, bool> _isValidParams =
            new Dictionary<string, bool>();

        /// <summary>
        /// Проверяет параметр на удовлетворение
        ///     требованиям предметной области.
        /// </summary>
        /// <param name="paramName">Название проверяемого параметра
        ///     фоторамки.</param>
        /// <param name="value">Значение проверяемого параметра
        ///     фоторамки.</param>
        /// <returns>Возвращает невыполненные требования
        ///     в проверяемом параметре.</returns>
        public string ValidateParameter(string paramName, double value)
        {
            var result = String.Empty;

            if (_methodsCheck.ContainsKey(paramName))
            {
                try
                {
                    _methodsCheck[paramName](value);
                }
                catch (ArgumentException ex)
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

        /// <summary>
        /// Проверяет удовлетворяют ли параметры фоторамки
        ///     требованиям предметной области.
        /// </summary>
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
                catch (ArgumentException ex)
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
        ///     ширины или длины.
        /// </summary>
        /// <param name="outerParam">Значение внешней ширины 
        ///     или длины.</param>
        /// <param name="outerParamName">Имя внешнего параметра
        ///     (использовать nameof для получения имени).</param>
        /// <param name="outerParamLabel">Название внешнего параметра
        ///     в родительном падеже.</param>
        /// <returns>Посчитанный внутренний параметр:
        ///     ширина или длина.</returns>
        /// <exception cref="ArgumentException">
        ///     Возникает, если нарушена зависимость между
        ///     внутренним и внешним параметром.</exception>
        private double CalcInnerParam(double outerParam,
            string outerParamName, string outerParamLabel)
        {
            var innerParam = outerParam - Interval * 2;

            if (!(innerParam > 0) || !(outerParam > innerParam))
            {
                var msg = $"Значение внешней {outerParamLabel}" +
                    $"= {outerParam} должно быть больше " +
                    $"двойного интервала = {Interval * 2}" +
                    $"({Interval}*2)";
                throw new ArgumentException(msg);
            }

            return innerParam;
        }

        /// <summary>
        /// Внутренняя ширина фоторамки.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает при нарушении связи
        ///     с внешней шириной фоторамки.</exception>
        public double InnerWidth
        {
            get
            {
                return CalcInnerParam(OuterWidth,
                    nameof(OuterWidth), "ширины");
            }
        }


        /// <summary>
        /// Внутренняя длина фоторамки.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает при нарушении связи
        ///     с внешней высотой фоторамки.</exception>
        public double InnerLength
        {
            get
            {
                return CalcInnerParam(OuterLength,
                    nameof(OuterLength), "длины");
            }
        }
    }


    /// <summary>
    /// Диапазон допустимых значений параметра.
    /// </summary>
    public class BorderConditions
    {
        private double _minimum = 0.0f;
        private double _maximum = 0.0f;
        private double _value = 0.0f;

        /// <summary>
        /// Установление граничных условий для значения.
        /// </summary>
        /// <param name="min">Минимальное значение.</param>
        /// <param name="value">Текущее значение.</param>
        /// <param name="max">Максимальное значение.</param>
        /// <exception cref="ArgumentException">
        ///     Возникает, если максимальное
        ///     меньше минимального значения.</exception>
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
        /// Максимальное значение диапазона.
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
        /// Минимальное значение диапазона.
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
        /// Значение, входящее в диапазон.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public double Value
        {
            get => _value;
            set
            {
                if (value > Max)
                {
                    var msg = $"Заданное значение = {value} больше," +
                        $" чем максимальное значение = {Max}";
                    throw new ArgumentException(msg);
                }

                if (value < Min)
                {
                    var msg = $"Заданное значение = {value} меньше," +
                        $" чем минимальное значение = {Min}";
                    throw new ArgumentException(msg);
                }

                _value = value;
            }
        }
    }
}
