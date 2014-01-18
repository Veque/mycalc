using System.Windows.Media;
using System.Windows.Threading;
using MVVM;
using MyCalc.Classes;

namespace MyCalc.VM {
	public class DistributionItemVM : ViewModelBase {

		private DistributionItem item;
		private double widthPerDot;
		private double maxHeight;

		public DistributionItemVM(DistributionItem item, Dispatcher dispatcher)
			: base(dispatcher) {
			Item = item;
		}

		private double percent { get { return ((double)Item.Count) / Item.Distribution.Total; } }

		public double WidthPerDot { get { return widthPerDot; } set { widthPerDot = value; OnPropertyChanged("CalculatedWidth"); } }
		public double MaxHeight { get { return maxHeight; } set { maxHeight = value; OnPropertyChanged("CalculatedHeight"); } }

		public DistributionItem Item {
			get { return item; }
			set {
				item = value;
				OnPropertiesChanged("Title", "PercentLabel", "CalculatedWidth", "CalculatedHeight");
			}
		}

		public string Title { get { return Item.Title; } }
		public string PercentLabel { get { return percent.ToString("p2"); } }
		public double CalculatedWidth { get { return (Item.HighValue - Item.LowValue + 1) * WidthPerDot; } }
		public double CalculatedHeight { get { return (percent / Item.Distribution.MaxPercent) * MaxHeight; } }

		private Brush color;

		public Brush Color {
			get {
				if (color == null)
					color = ColorRandomizer.PickColor();
				return color;
			}
			set {
				color = value;
				OnPropertiesChanged("Color");
			}
		}
	}
}
