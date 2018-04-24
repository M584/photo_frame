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
        private IBuilder _builder;

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

        public bool BuildModel()
        {
            var resultBuilding = true;
            try
            {
                _builder.Build(_photoFrame as IPhotoFrame,
                _photoFrame as IChecker);
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
