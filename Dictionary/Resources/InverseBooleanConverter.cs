using System;
using Windows.UI.Xaml.Data;

namespace Dictionary.Resources
{
    /// <summary>
    /// Converter for inverting a boolean value.
    /// </summary>
    class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Invert a boolean value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The culture to use in the converter.</param>
        /// <returns>The converted object.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
