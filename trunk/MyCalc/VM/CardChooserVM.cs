using System.Collections.ObjectModel;
using System.Windows.Threading;
using MyCalc.Classes;

namespace MyCalc.VM {
	public class CardChooserVM : ViewModelBase {
		public CardChooserVM(Dispatcher dispatcher) : base(dispatcher) { }

		public ObservableCollection<CardInGridVM> Cards {
			get {
				var res = new ObservableCollection<CardInGridVM>();
				for (int i = 0; i < 13; i++) {
					for (int j = 0; j < 4; j++) {
						res.Add(new CardInGridVM(new Card((ushort)(i + 1), (ushort)(j + 1)), UIDispatcher));
					}
				}
				return res;
			}
		}

		private CardInGridVM selectedCard;

		public CardInGridVM SelectedCard{
			get { return selectedCard; }
			set{
				selectedCard = value;
				OnPropertiesChanged("SelectedCard");
			}
		}
	}
}