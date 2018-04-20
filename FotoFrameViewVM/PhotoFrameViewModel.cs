using System;
using System.ComponentModel;
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
            const double minForLengthAndWidth = 10.0f;
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
            }
        }

        public double OuterHeight {
            get
            {
                return _photoFrame.OuterHeight;
            }
            set
            {
                _photoFrame.OuterHeight = value;
            }
        }

        public double OuterLength {
            get
            {
                return _photoFrame.OuterLength;
            }
            set
            {
                _photoFrame.OuterLength = value;
            }
        }

        public double InnerHeight {
            get
            {
                return _photoFrame.InnerHeight;
            }
            set
            {
                _photoFrame.InnerHeight = value;
            }
        }

        public double Interval {
            get
            {
                return _photoFrame.Interval;
            }
            set
            {
                _photoFrame.Interval = value;
            }
        }
    }
}
