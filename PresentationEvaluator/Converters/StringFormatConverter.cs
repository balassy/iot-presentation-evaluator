using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace PresentationEvaluator.Converters
{
	public sealed class StringFormatConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, string language )
		{
			if( value == null )
			{
				return null;
			}

			if( parameter == null )
			{
				return value;
			}

			return String.Format( CultureInfo.CurrentCulture, (string) parameter, value );
		}

		public object ConvertBack( object value, Type targetType, object parameter, string language )
		{
			throw new NotImplementedException();
		}
	}
}
