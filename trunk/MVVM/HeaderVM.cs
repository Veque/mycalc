using System;
using System.Windows.Threading;

namespace MVVM {
	public class HeaderVM : ViewModelBase {

		public HeaderVM(Dispatcher dispatcher) : base(dispatcher) { }

		private string header;

		public string Header {
			get { return header; }
			set {
				header = value;
				OnPropertiesChanged("Header");
			}
		}

		public event EventHandler CloseTab;

		protected virtual void OnCloseTab() {
			var handler = CloseTab;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public Command CloseTabCommand { get { return new Command(s => OnCloseTab()); } }
	}
}