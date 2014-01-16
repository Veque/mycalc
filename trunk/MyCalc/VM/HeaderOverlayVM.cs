using System;
using System.Windows.Threading;

namespace MyCalc.VM {
	public class HeaderOverlayVM : HeaderVM {
		public HeaderOverlayVM(Dispatcher dispatcher)
			: base(dispatcher) {
			overlayText = "Подождите...";
		}

		private bool showOverlay;

		public bool ShowOverlay {
			get { return showOverlay; }
			set {
				showOverlay = value;
				UIDispatcher.Invoke(() => OnPropertiesChanged("ShowOverlay"));
			}
		}

		private string overlayText;

		public string OverlayText {
			get { return overlayText; }
			set {
				overlayText = value;
				UIDispatcher.Invoke(() => OnPropertiesChanged("OverlayText"));
			}
		}
	}
}