using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CapstoneDesktop.Converters
{
    /// <summary>
    ///     DateFormatter to be used when displaying dates
    /// </summary>
    /// <seealso cref="Avalonia.Data.Converters.IValueConverter" />
    public class DateFormatter : IValueConverter
    {
        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target.</param>
        /// <param name="parameter">A user-defined parameter.</param>
        /// <param name="culture">The culture to use.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        /// <remarks>
        ///     This method should not throw exceptions. If the value is not convertible, return
        ///     a <see cref="T:Avalonia.Data.BindingNotification" /> in an error state. Any exceptions thrown will be
        ///     treated as an application exception.
        /// </remarks>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime) value;
            DataShown type;

            if (Enum.TryParse((string) parameter, out type))
                switch (type)
                {
                    case DataShown.Both:
                        return date.ToString("MM/dd/yyyy hh:mm tt", culture);
                    case DataShown.Date:
                        return date.ToString("MM/dd/yyyy", culture);
                    case DataShown.Time:
                        return date.ToString("hh:mm tt", culture);
                    default:
                        return date.ToString(culture);
                }

            return date.ToString(culture);
        }

        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target.</param>
        /// <param name="parameter">A user-defined parameter.</param>
        /// <param name="culture">The culture to use.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <remarks>
        ///     This method should not throw exceptions. If the value is not convertible, return
        ///     a <see cref="T:Avalonia.Data.BindingNotification" /> in an error state. Any exceptions thrown will be
        ///     treated as an application exception.
        /// </remarks>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private enum DataShown
        {
            Date,
            Time,
            Both
        }
    }
}