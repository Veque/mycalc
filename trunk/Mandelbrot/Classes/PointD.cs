namespace Mandelbrot.Classes{
	public class PointD {
		public double X { get; set; }
		public double Y { get; set; }

		public double SqDist { get { return X * X + Y * Y; } }
	}
}