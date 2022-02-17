using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CapstoneDesktop.Converters
{
    public class DateFormatter : IValueConverter
    {
        
        private enum DataShown
        {
            Date,
            Time,
            Both
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime) value;
            DataShown type;
            
            if (Enum.TryParse((string) parameter, out type))
            {
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
            }
            return date.ToString(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}