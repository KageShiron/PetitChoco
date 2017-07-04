using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PetitChoco.Converters
{
    public sealed class TryParseConverter : IValueConverter
    {
        private readonly static Dictionary<Type, IValueConverter> converters = new Dictionary<Type, IValueConverter>();

        private static IValueConverter getConverter(Type targetType)
        {
            IValueConverter converter;
            if (converters.TryGetValue(targetType, out converter))
                return converter;
            converter = Activator.CreateInstance(typeof(TryParseConverter<>).MakeGenericType(targetType)) as IValueConverter;
            if (converter == null)
                throw new InvalidCastException(string.Format("型 {0}は、{1}を継承していません。", targetType, nameof(IValueConverter)));
            converters.Add(targetType, converter);
            return converter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null ? null : getConverter(value.GetType()).Convert(value, targetType, parameter, culture);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => getConverter(targetType).ConvertBack(value, targetType, parameter, culture);
    }

    public sealed class TryParseConverter<T> : IValueConverter
    {
        private delegate bool TryParse(string input, out T value);
        private static readonly TryParse _tryParse;

        static TryParseConverter()
        {
            var tryParse = typeof(T).GetMethod(nameof(Int32.TryParse), new[] { typeof(string), typeof(T).MakeByRefType() });
            if (tryParse == null)
                throw new TypeAccessException("TryParseメソッドが見つかりません。");
            var value = Expression.Parameter(typeof(T).MakeByRefType());
            var input = Expression.Parameter(typeof(string));
            _tryParse = Expression.Lambda<TryParse>(Expression.Call(tryParse, input, value), input, value).Compile();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // to string
            if (value == null)
                return null;
            if (value is IFormattable && parameter is string)
                return ((IFormattable)value).ToString((string)parameter, null);
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // to object
            if (value == null)
                return null;
            T tmp;
            _tryParse(value as string, out tmp);
            return tmp;
        }
    }
}
