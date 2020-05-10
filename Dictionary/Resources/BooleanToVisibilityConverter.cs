﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Dictionary.Resources
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility))
            {
                throw new InvalidOperationException("Target type must be Visibility!");
            }
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
