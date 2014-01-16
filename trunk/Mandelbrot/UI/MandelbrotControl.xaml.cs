using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mandelbrot.UI {
	/// <summary>
	/// Interaction logic for MandelbrotControl.xaml
	/// </summary>
	public partial class MandelbrotControl : UserControl {
		public MandelbrotControl() {
			InitializeComponent();
			image.MouseDown += (sender, e) => {
				var position = e.GetPosition(image);
				var width = frame.ActualWidth;
				var height = frame.ActualHeight;

				var left = Math.Max(position.X - width / 2, 0);
				var top = Math.Max(position.Y - height / 2, 0);
				left = Math.Min(left, image.ActualWidth - width);
				top = Math.Min(top, image.ActualHeight - height);
				frame.SetValue(Canvas.LeftProperty, left);
				frame.SetValue(Canvas.TopProperty, top);
			};
		}

	}
}
