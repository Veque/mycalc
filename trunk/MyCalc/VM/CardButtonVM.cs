using System.Windows.Threading;
using MyCalc.Classes;

namespace MyCalc.VM {
	public class CardButtonVM : CardVM {
		public CardButtonVM(Card card, Dispatcher dispatcher) : base(card, dispatcher) { }

		public Command ChooseCardCommand {
			get {
				return new Command(s => {
					this.card = CardChooser.ChooseCard(UIDispatcher).card;
				});
			}
		}
	}
}