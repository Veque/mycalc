using System.Windows.Media.Imaging;

namespace Mandelbrot.Classes
{
	public class MandelbrotImage
	{
		public RectangleD Field { get; set; }
		public WriteableBitmap Image { get; set; }
	}
}