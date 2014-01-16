using System.Drawing;
using System.Windows.Media;
using System.Windows.Threading;
using MyCalc.Classes;
using Brush = System.Windows.Media.Brush;

namespace MyCalc.VM {
	public class CardVM : ViewModelBase {
		private Card _card;

		public CardVM(Card card, Dispatcher dispatcher)
			: base(dispatcher) {
			this.card = card;
		}

		public Card card {
			get { return _card; }
			protected set {
				_card = value;
				OnPropertiesChanged("SuitColor", "SuitImage", "ValueText");
			}
		}

		public string ValueText {
			get { return card.IsEmpty ? "-" : CardResources.Values[card.Value - 1]; }
		}

		public Brush SuitColor { get { return card.IsEmpty ? System.Windows.Media.Brushes.DarkGray : CardResources.SuitColors[card.Suit - 1]; } }
		public ImageSource SuitImage { get { return card.IsEmpty ? null : CardResources.Suits[card.Suit - 1]; } }
	}
}