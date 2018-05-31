using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FotoFrameModel;

namespace FotoFrameViewVM
{
    /// <summary>
    /// Представление модели фоторамки.
    /// </summary>
    public class PhotoFrameViewModel : INotifyPropertyChanged,
        IDataErrorInfo
    {
        /// <summary>
        /// Шаблон фоторамки.
        /// </summary>
        private IPhotoFrame _photoFrame;

        /// <summary>
        /// Построитель фоторамки.
        /// </summary>
        private IBuilder _builder;

        /// <summary>
        /// Инициализация параметров представления модели.
        /// </summary>
        public PhotoFrameViewModel()
        {
            const double minHeight = 1.0f;
            const double minForLengthAndWidth = 5.0f;
            const double max = 100.0f;
            const double maxInterval = 6.0f;
            const double value = 5.0f;

            var outerWidth = new BorderConditions(minForLengthAndWidth,
                max, max);
            var outerHeight = new BorderConditions(minHeight,
                value, minForLengthAndWidth);
            var outerLength = new BorderConditions(minForLengthAndWidth,
                max, max);
            var innerHeight = new BorderConditions(minHeight,
                value, minForLengthAndWidth);
            var interval = new BorderConditions(minHeight,
                value, maxInterval);

            _photoFrame = new PhotoFrameTemplate(outerWidth,
                outerHeight,
                outerLength,
                innerHeight,
                interval);

            _builder = new BuilderPhotoFrame();
        }

        /// <summary>
        /// Внешняя ширина фоторамки.
        /// </summary>
        public double OuterWidth
        {
            get
            {
                _photoFrame.OuterWidth = _photoFrame.OuterWidth;
                return _photoFrame.OuterWidth;
            }
            set
            {
                _photoFrame.OuterWidth = value;
                OnPropertyChanged(nameof(this.OuterWidth));
                OnPropertyChanged(nameof(this.Interval));
            }
        }

        /// <summary>
        /// Внешняя высота фоторамки.
        /// </summary>
        public double OuterHeight
        {
            get
            {
                return _photoFrame.OuterHeight;
            }
            set
            {
                _photoFrame.OuterHeight = value;
                OnPropertyChanged(nameof(this.OuterHeight));
                OnPropertyChanged(nameof(this.InnerHeight));
            }
        }

        /// <summary>
        /// Внешняя длина фоторамки.
        /// </summary>
        public double OuterLength
        {
            get
            {
                _photoFrame.OuterLength = _photoFrame.OuterLength;
                return _photoFrame.OuterLength;
            }
            set
            {
                _photoFrame.OuterLength = value;
                OnPropertyChanged(nameof(this.OuterLength));
                OnPropertyChanged(nameof(this.Interval));
            }
        }

        /// <summary>
        /// Внутренняя высота фоторамки.
        /// </summary>
        public double InnerHeight
        {
            get
            {
                return _photoFrame.InnerHeight;
            }
            set
            {
                _photoFrame.InnerHeight = value;
                OnPropertyChanged(nameof(this.InnerHeight));
                OnPropertyChanged(nameof(this.OuterHeight));
            }
        }

        /// <summary>
        /// Расстояние между внутренними и 
        ///     внешними параметрами фоторамки (длиной и шириной).
        /// </summary>
        public double Interval
        {
            get
            {
                _photoFrame.Interval = _photoFrame.Interval;
                return _photoFrame.Interval;
            }
            set
            {
                _photoFrame.Interval = value;
                OnPropertyChanged(nameof(this.Interval));
                OnPropertyChanged(nameof(this.OuterWidth));
                OnPropertyChanged(nameof(this.OuterLength));
            }
        }

        /// <summary>
        /// Построить модель фоторамки по шаблонки.
        /// </summary>
        /// <returns>Возвращает удалось 
        /// или нет построить модель фоторамки в САПР.</returns>
        public bool BuildModel()
        {
            var resultBuilding = true;
            try
            {
                _builder.Build(_photoFrame);
            }
            catch (InvalidOperationException)
            {
                resultBuilding = false;
            }

            return resultBuilding;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion


        #region IDataErrorInfo Members

        public string this[string columnName]
        {
            get
            {
                return String.Empty;
            }
        }

        public string Error => null;

        #endregion
    }
}
