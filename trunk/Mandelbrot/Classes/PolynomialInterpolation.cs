using System.Collections.Generic;
using System.Linq;

namespace Mandelbrot.Classes {
	public class Interpolation {
		private List<PointD> _hooks;
		public List<PointD> Hooks {
			get { return _hooks; }
			set {
				_hooks = value;
				if (_hooks != null)
					_hooks.Sort((h1, h2) => h1.X.CompareTo(h2.X));
			}
		}

		public Interpolation()
		{
			Hooks = new List<PointD>();
		}

		public double F(double x) {
			var hooksCount = Hooks.Count;
			for (int i = 0; i < hooksCount - 1; i++) {
				var hook1 = Hooks[i];
				var hook2 = Hooks[i + 1];
				var x1 = hook1.X;

				if (i == 0 && x < x1)
					return hook1.Y;

				var x2 = hook2.X;

				if (i == hooksCount - 2 && x > x2)
					return hook2.Y;

				if (x1 <= x && x <= x2) {
					var y1 = hook1.Y;
					var y2 = hook2.Y;
					var t = (x - x1) / (x2 - x1);
					var a = y1 - y2;
					var b = -a;
					return (1 - t) * y1 + t * y2 + t * (1 - t) * (a * (1 - t) + b * t);
				}
			}
			return 0;
		}
	}
}