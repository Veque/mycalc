using System.Windows.Threading;
using MyCalc.Classes;

namespace MyCalc.VM {
	public class CardInGridVM : CardVM {
		public CardInGridVM(Card card, Dispatcher dispatcher)
			: base(card, dispatcher) {
		}

		public int Row { get { return card.IsEmpty ? 0 : card.Suit - 1; } }
		public int Column { get { return card.IsEmpty ? 0 : card.Value - 1; } }
	}
}