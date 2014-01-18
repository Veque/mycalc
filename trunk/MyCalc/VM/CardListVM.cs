using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using MVVM;
using MyCalc.Classes;

namespace MyCalc.VM {
	public class CardListVM : ViewModelBase {
		public CardListVM(int count, Dispatcher dispatcher)
			: base(dispatcher) {
			_count = count;
			var list = new List<CardButtonVM>();
			for (int i = 0; i < _count; i++) {
				list.Add(new CardButtonVM(new Card(), UIDispatcher));
			}
			Cards = new ObservableCollection<CardButtonVM>(list);
		}
		protected int _count;

		private ObservableCollection<CardButtonVM> cards;

		public ObservableCollection<CardButtonVM> Cards {
			get { return cards; }
			set {
				cards = value;
				OnPropertiesChanged("Cards");
			}
		}
	}
}