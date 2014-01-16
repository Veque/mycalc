using System;

namespace Mandelbrot.Classes {
	public class MandelbrotCalculator {
		public static int[,] Calculate(RectangleD rect, int discreteWidth, int discreteHeight, double limit, int maxIterations) {
			var widthStep = rect.Width / discreteWidth;
			var halfWidthStep = widthStep / 2;
			var heightStep = rect.Height / discreteHeight;
			var halfHeightStep = heightStep / 2;

			var res = new int[discreteWidth, discreteHeight];

			for (int x = 0; x < discreteWidth; x++) {
				for (int y = 0; y < discreteHeight; y++) {
					var dX = rect.Left + x * widthStep + halfWidthStep;
					var dY = rect.Top + y * heightStep + halfHeightStep;

					var iterations = 1;
					var pointX = dX;
					var pointY = dY;
					var newX = 0d;

					while (
						pointX < limit &&
						-pointX < limit &&
						pointY < limit &&
						-pointY < limit &&
						//Math.Abs(pointX) < sqLimit && Math.Abs(pointY) < limit &&
						iterations < maxIterations) {
						newX = pointX * pointX - pointY * pointY + dX;
						pointY = 2 * pointX * pointY + dY;
						pointX = newX;

						//f.X = newX;
						//f.Y = newY;
						//dist = f.SqDist;
						iterations++;
					}
					res[x, y] = iterations;
				}
			}
			return res;
		}
	}
}