using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CapstoneDesktop.Converters
{
    /// <summary>
    ///     DateFormatter used when displaying dates
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
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            var date = (DateTime) value;
            if (!Enum.TryParse((string) parameter, out DataShown type)) return date.ToString(culture);

            return type switch
            {
                DataShown.Both => date.ToString("MM/dd/yyyy hh:mm tt", culture),
                DataShown.Date => date.ToString("MM/dd/yyyy", culture),
                DataShown.Time => date.ToString("hh:mm tt", culture),
                _ => date.ToString(culture)
            };
        }

        /// <summary>
        ///     Converts a value back.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target.</param>
        /// <param name="parameter">A user-defined parameter.</param>
        /// <param name="culture">The culture to use.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Since the method is not implemented.</exception>
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