using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FotoFrameModel;

namespace FotoFrameViewVM
{
    public class PhotoFrameViewModel : INotifyPropertyChanged,
        IDataErrorInfo
    {
        private PhotoFrameTemplate _photoFrame;

        public PhotoFrameViewModel()
        {
            const double minHeight = 1.0f;
            const double minForLengthAndWidth = 15.0f;
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
        }

        public double OuterWidth
        {
            get
            {
                return _photoFrame.OuterWidth;
            }
            set
            {
                _photoFrame.OuterWidth = value;
                OnPropertyChanged(nameof(this.OuterWidth));
            }
        }

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
            }
        }

        public double OuterLength
        {
            get
            {
                return _photoFrame.OuterLength;
            }
            set
            {
                _photoFrame.OuterLength = value;
                OnPropertyChanged(nameof(this.OuterLength));
            }
        }

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
            }
        }

        public double Interval
        {
            get
            {
                return _photoFrame.Interval;
            }
            set
            {
                _photoFrame.Interval = value;
                OnPropertyChanged(nameof(this.Interval));
            }
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
                //TODO: заменить на реализацию проверки параметра
                return String.Empty;
            }
        }

        public string Error => null;

        #endregion
    }
}
