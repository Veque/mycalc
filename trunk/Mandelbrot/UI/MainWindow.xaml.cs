using System.Windows;
using Mandelbrot.VM;

namespace Mandelbrot.UI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			var vm = new MandelbrotVM(this.Dispatcher);
			this.DataContext = vm;
		}
	}
}
