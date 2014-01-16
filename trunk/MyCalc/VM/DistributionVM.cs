using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MyCalc.Classes;

namespace MyCalc.VM {
	public class DistributionVM : HeaderOverlayVM {
		private Distribution distribution;

		public DistributionVM(Dispatcher dispatcher)
			: base(dispatcher) {
		}

		public double WidthPerDot { get; set; }
		public double MaxHeight { get; set; }

		public Distribution Distribution {
			get { return distribution; }
			set {
				distribution = value;
				OnPropertiesChanged("Distribution", "Items", "AverageMargin", "AverageText");
			}
		}

		public ObservableCollection<DistributionItemVM> Items {
			get {
				if (distribution == null) return new ObservableCollection<DistributionItemVM>();
				return new ObservableCollection<DistributionItemVM>(
					distribution.Items.Select(d => new DistributionItemVM(d, this.UIDispatcher) {
						WidthPerDot = this.WidthPerDot,
						MaxHeight = this.MaxHeight
					}));
			}
		}

		public string AverageText {
			get {
				if (distribution == null)
					return string.Empty;
				return string.Format("{0:n1}", distribution.Average);
			}
		}

		public Thickness AverageMargin {
			get {
				if (distribution == null)
					return new Thickness(0);
				return new Thickness(distribution.Average * WidthPerDot, 0, 0, 0);
			}
		}
	}
}