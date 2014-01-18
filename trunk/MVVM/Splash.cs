using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MVVM {
	public class Splash<TWindow> : INotifyPropertyChanged, IDisposable where TWindow : Window, new() {

		private string message;

		public string Message {
			get { return message; }
			set {
				message = value;
				OnPropertyChanged();
			}
		}

		private readonly TWindow window;
		public Splash(){
			message = "Подожите...";
			window = new TWindow { DataContext = this };
			window.Show();
		}

		public void SetMessage(string message) {
			window.Dispatcher.InvokeAsync(new Action(() => Message = message));
		}

		public void Dispose() {
			if (window != null)
				window.Close();
			GC.SuppressFinalize(this);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}