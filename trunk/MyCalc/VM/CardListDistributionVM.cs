using System.Threading;
using System.Windows.Threading;
using MVVM;
using MyCalc.Classes;
using System.Linq;
using MyCalc.UI;

namespace MyCalc.VM {
	public class CardListDistributionVM : HeaderOverlayVM {
		public CardListDistributionVM(Dispatcher dispatcher)
			: base(dispatcher) {
			Header = "Распределение";
			CardList = new CardListVM(2, dispatcher);
			Distribution = new DistributionVM(dispatcher);
			Distribution.WidthPerDot = 0.2;
			Distribution.MaxHeight = 500;
		}

		private CardListVM cardList;

		public CardListVM CardList {
			get { return cardList; }
			set {
				cardList = value;
				OnPropertiesChanged("CardList");
			}
		}

		private DistributionVM distribution;

		public DistributionVM Distribution {
			get { return distribution; }
			set {
				distribution = value;
				OnPropertiesChanged("Distribution");
			}
		}

		public Command CalculateCommand {
			get {
				return new Command(
					s => {
						var list = cardList.Cards.Where(c => !c.card.IsEmpty).Select(c => c.card).ToList();
						if (list.Count > 1) {
							var calc = new DistributionCalculator();
							calc.Notify += (sender, e) => this.OverlayText = e.Message;


							var thread = new Thread(
								() => {
									this.ShowOverlay = true;
									try {
										var distribution = calc.CalculateDistributionForPair(list[0].Value, list[0].Suit, list[1].Value, list[1].Suit);
										Distribution.Distribution = distribution;
									} finally {
										ShowOverlay = false;
									}
								});

							thread.Start();

						}

					});

			}
		}
	}
}