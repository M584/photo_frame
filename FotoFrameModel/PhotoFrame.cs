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
        public PhotoFrameTemplate(
            BorderConditions outerWidth,
            BorderConditions outerHeight,
            BorderConditions outerLength,
            BorderConditions InnerHeight,
            BorderConditions Interval)
        {
            throw new NotImplementedException();
        }

        public double OuterWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double OuterHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double OuterLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double InnerHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Interval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double InnerWidth => throw new NotImplementedException();

        public double InnerLength => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();
    }

    
    public class BorderConditions
    {
        private double _minimum = 0.0f;
        private double _maximum = 0.0f;
        private double _value = 0.0f;

        public BorderConditions(double min, double value, double max)
        {
            throw new NotImplementedException();
        }

        public double Max { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Min { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}