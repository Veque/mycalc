using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MVVM {
	public class Command : ICommand {

		#region Fields
		readonly Action<object> _execute;
		readonly Predicate<object> _canExecute;
		#endregion // Fields

		#region Constructors
		public Command(Action<object> execute) : this(execute, null) { }
		public Command(Action<object> execute, Predicate<object> canExecute) {
			if (execute == null) throw new ArgumentNullException("execute");
			_execute = execute;
			_canExecute = canExecute;
		}
		#endregion // Constructors

		#region ICommand Members

		[DebuggerStepThrough]
		public bool CanExecute(object parameter) {
			return _canExecute == null || _canExecute(parameter);
		}

		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter) {
			_execute(parameter);
		}
		#endregion // ICommand Members
	}
}