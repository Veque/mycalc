using System.Windows.Data;
using System.Windows.Media;
using System.Linq;
using System.Reflection;

namespace Mandelbrot.Resources {
	public class ColorUIConverter : IValueConverter {

		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			var brush = value as SolidColorBrush;
			if (brush != null) {
				var color = brush.Color;
				var type = typeof(Colors);
				var properties = type.GetProperties().ToList();
				foreach (var pi in properties) {
					var c = (Color)pi.GetValue(null);
					if (c == color)
						return pi;
				}
			}
			return null;
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (value is PropertyInfo)
			{
				var pi = value as PropertyInfo;
				var color = (Color)ColorConverter.ConvertFromString(pi.Name);
				return new SolidColorBrush(color);
			}
			if (value is SolidColorBrush)
				return value;
			return default(SolidColorBrush);
		}
	}
}