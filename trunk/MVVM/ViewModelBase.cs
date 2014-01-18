using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace MVVM {
	public class ViewModelBase : INotifyPropertyChanged {

		public ViewModelBase(Dispatcher dispatcher){
			UIDispatcher = dispatcher;
		}

		public Dispatcher UIDispatcher { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName) {

			AssertPropertyName(propertyName);

			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		private void AssertPropertyName(string propertyName) {
#if DEBUG
			if (this.GetType().GetProperty(propertyName) == null)
				throw new ArgumentException("No such property in this class", propertyName);
#endif
		}

		protected virtual void OnPropertiesChanged(params string[] propertyNames) {
			foreach (var propertyName in propertyNames) {
				OnPropertyChanged(propertyName);
			}
		}
	}
}