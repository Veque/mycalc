using System;
using System.Collections.Generic;

namespace MyCalc.Classes {
	public class DistributionCalculator {

		public class NotifyEventArgs : EventArgs {
			public NotifyEventArgs(string message) {
				Message = message;
			}

			public string Message { get; private set; }
		}

		public event EventHandler<NotifyEventArgs> Notify;

		protected virtual void OnNotify(string e) {
			var handler = Notify;
			if (handler != null) handler(this, new NotifyEventArgs(e));
		}

		public Distribution CalculateCombinationsDistribution() {
			var start = DateTime.Now;
			var highsItem = new DistributionItem() { Title = "Highs", LowValue = Const.HIGHS_LOW7, HighValue = Const.HIGHS_HIGH7 };
			var pairsItem = new DistributionItem() { Title = "Pairs", LowValue = Const.PAIRS_LOW7, HighValue = Const.PAIRS_HIGH7 };
			var twoPairsItem = new DistributionItem() { Title = "Tow Pairs", LowValue = Const.TWO_PAIRS_LOW7, HighValue = Const.TWO_PAIRS_HIGH7 };
			var threesItem = new DistributionItem() { Title = "Threes", LowValue = Const.THREES_LOW7, HighValue = Const.THREES_HIGH7 };
			var streetsItem = new DistributionItem() { Title = "Streets", LowValue = Const.STREET_LOW7, HighValue = Const.STREET_HIGH7 };
			var flushesItem = new DistributionItem() { Title = "Flushes", LowValue = Const.FLUSH_LOW7, HighValue = Const.FLUSH_HIGH7 };
			var fullHousesItem = new DistributionItem() { Title = "Full Houses", LowValue = Const.FULL_HOUSE_LOW7, HighValue = Const.FULL_HOUSE_HIGH7 };
			var foursItem = new DistributionItem() { Title = "Fours", LowValue = Const.FOURS_LOW7, HighValue = Const.FOURS_HIGH7 };
			var streetFlushesItem = new DistributionItem() { Title = "Street Flushes", LowValue = Const.STREET_FLUSH_LOW7, HighValue = Const.STREET_FLUSH_HIGH7 };

			var distribution = new Distribution() {
				Items = new List<DistributionItem>(
					new[]{
						highsItem,
						pairsItem,
						twoPairsItem,
						threesItem,
						streetsItem,
						flushesItem,
						fullHousesItem,
						foursItem,
						streetFlushesItem
					})
			};

			highsItem.Distribution = distribution;
			pairsItem.Distribution = distribution;
			twoPairsItem.Distribution = distribution;
			threesItem.Distribution = distribution;
			streetsItem.Distribution = distribution;
			flushesItem.Distribution = distribution;
			fullHousesItem.Distribution = distribution;
			foursItem.Distribution = distribution;
			streetFlushesItem.Distribution = distribution;
			var total = Combinatorics.C(52, 7);
			var deck = 52;

			var cardsList = new Card[7];

			for (int i1 = 0; i1 < deck; i1++) {
				var v1 = (ushort)(i1 / 4 + 1);
				var s1 = (ushort)(i1 % 4 + 1);
				for (int i2 = i1 + 1; i2 < deck; i2++) {
					var v2 = (ushort)(i2 / 4 + 1);
					var s2 = (ushort)(i2 % 4 + 1);
					for (int i3 = i2 + 1; i3 < deck; i3++) {
						var v3 = (ushort)(i3 / 4 + 1);
						var s3 = (ushort)(i3 % 4 + 1);
						for (int i4 = i3 + 1; i4 < deck; i4++) {
							var v4 = (ushort)(i4 / 4 + 1);
							var s4 = (ushort)(i4 % 4 + 1);
							for (int i5 = i4 + 1; i5 < deck; i5++) {
								var v5 = (ushort)(i5 / 4 + 1);
								var s5 = (ushort)(i5 % 4 + 1);
								for (int i6 = i5 + 1; i6 < deck; i6++) {
									var v6 = (ushort)(i6 / 4 + 1);
									var s6 = (ushort)(i6 % 4 + 1);
									//Parallel.For(i6 + 1, deck, i7 => {
									for (int i7 = i6 + 1; i7 < deck; i7++) {
										cardsList[0] = new Card(v1, s1);
										cardsList[1] = new Card(v2, s2);
										cardsList[2] = new Card(v3, s3);
										cardsList[3] = new Card(v4, s4);
										cardsList[4] = new Card(v5, s5);
										cardsList[5] = new Card(v6, s6);
										cardsList[6] = new Card((ushort)(i7 / 4 + 1), (ushort)(i7 % 4 + 1));
										var comb = new Combination(cardsList);
										var energy = comb.GetCachedEnergy();
										distribution.Total++;
										if (energy >= Const.STREET_FLUSH_LOW7)
											streetFlushesItem.Count++;
										else if (energy >= Const.FOURS_LOW7)
											foursItem.Count++;
										else if (energy >= Const.FULL_HOUSE_LOW7)
											fullHousesItem.Count++;
										else if (energy >= Const.FLUSH_LOW7)
											flushesItem.Count++;
										else if (energy >= Const.STREET_LOW7)
											streetsItem.Count++;
										else if (energy >= Const.THREES_LOW7)
											threesItem.Count++;
										else if (energy >= Const.TWO_PAIRS_LOW7)
											twoPairsItem.Count++;
										else if (energy >= Const.PAIRS_LOW7)
											pairsItem.Count++;
										else if (energy >= Const.HIGHS_LOW7)
											highsItem.Count++;
									}
								}
							}
						}
					}
					OnNotify(string.Format("Готово на {0:p2}... {1:n2}", distribution.Total / (double)total, (DateTime.Now - start).TotalSeconds));
				}
			}
			return distribution;
		}

		public Distribution CalculateDistributionForPair(int value1, int suit1, int value2, int suit2) {
			var highsItem = new DistributionItem() { Title = "Highs", LowValue = Const.HIGHS_LOW7, HighValue = Const.HIGHS_HIGH7 };
			var pairsItem = new DistributionItem() { Title = "Pairs", LowValue = Const.PAIRS_LOW7, HighValue = Const.PAIRS_HIGH7 };
			var twoPairsItem = new DistributionItem() { Title = "Tow Pairs", LowValue = Const.TWO_PAIRS_LOW7, HighValue = Const.TWO_PAIRS_HIGH7 };
			var threesItem = new DistributionItem() { Title = "Threes", LowValue = Const.THREES_LOW7, HighValue = Const.THREES_HIGH7 };
			var streetsItem = new DistributionItem() { Title = "Streets", LowValue = Const.STREET_LOW7, HighValue = Const.STREET_HIGH7 };
			var flushesItem = new DistributionItem() { Title = "Flushes", LowValue = Const.FLUSH_LOW7, HighValue = Const.FLUSH_HIGH7 };
			var fullHousesItem = new DistributionItem() { Title = "Full Houses", LowValue = Const.FULL_HOUSE_LOW7, HighValue = Const.FULL_HOUSE_HIGH7 };
			var foursItem = new DistributionItem() { Title = "Fours", LowValue = Const.FOURS_LOW7, HighValue = Const.FOURS_HIGH7 };
			var streetFlushesItem = new DistributionItem() { Title = "Street Flushes", LowValue = Const.STREET_FLUSH_LOW7, HighValue = Const.STREET_FLUSH_HIGH7 };

			var distribution = new Distribution() {
				Items = new List<DistributionItem>(
					new[]{
						highsItem,
						pairsItem,
						twoPairsItem,
						threesItem,
						streetsItem,
						flushesItem,
						fullHousesItem,
						foursItem,
						streetFlushesItem
					})
			};

			highsItem.Distribution = distribution;
			pairsItem.Distribution = distribution;
			twoPairsItem.Distribution = distribution;
			threesItem.Distribution = distribution;
			streetsItem.Distribution = distribution;
			flushesItem.Distribution = distribution;
			fullHousesItem.Distribution = distribution;
			foursItem.Distribution = distribution;
			streetFlushesItem.Distribution = distribution;
			var total = Combinatorics.C(52, 5);
			var deck = 52;

			var swap = value1 > value2;
			if (swap) {
				var tmp = value1;
				value1 = value2;
				value2 = tmp;

				tmp = suit1;
				suit1 = suit2;
				suit2 = tmp;
			}
			var i6 = (value1 - 1) * 4 + suit1 - 1;
			var i7 = (value2 - 1) * 4 + suit2 - 1;
			var v6 = (ushort)value1;
			var v7 = (ushort)value2;
			var s6 = (ushort)suit1;
			var s7 = (ushort)suit2;

			var cardsList = new Card[7];

			var average = 0d;
			for (int i1 = 0; i1 < deck; i1++) {
				if (i1 == i6 || i1 == i7) continue;
				var v1 = (ushort)(i1 / 4 + 1);
				var s1 = (ushort)(i1 % 4 + 1);
				for (int i2 = i1 + 1; i2 < deck; i2++) {
					if (i2 == i6 || i2 == i7) continue;
					var v2 = (ushort)(i2 / 4 + 1);
					var s2 = (ushort)(i2 % 4 + 1);
					for (int i3 = i2 + 1; i3 < deck; i3++) {
						if (i3 == i6 || i3 == i7) continue;
						var v3 = (ushort)(i3 / 4 + 1);
						var s3 = (ushort)(i3 % 4 + 1);
						for (int i4 = i3 + 1; i4 < deck; i4++) {
							if (i4 == i6 || i4 == i7) continue;
							var v4 = (ushort)(i4 / 4 + 1);
							var s4 = (ushort)(i4 % 4 + 1);
							for (int i5 = i4 + 1; i5 < deck; i5++) {
								if (i5 == i6 || i5 == i7) continue;
								//for (int i6 = i5 + 1; i6 < deck; i6++) {
								//Parallel.For(i6 + 1, deck, i7 => {
								//for (int i7 = i6 + 1; i7 < deck; i7++) {
								var n1 = 0;
								var n2 = 1;
								var n3 = 2;
								var n4 = 3;
								var n5 = 4;
								var n6 = 5;
								var n7 = 6;

								if (i6 < i5) {
									n6--;
									n5++;
								}

								if (i6 < i4) {
									n6--;
									n4++;
								}

								if (i6 < i3) {
									n6--;
									n3++;
								}

								if (i6 < i2) {
									n6--;
									n2++;
								}

								if (i6 < i1) {
									n6--;
									n1++;
								}

								if (i7 < i5) {
									n7--;
									n5++;
								}

								if (i7 < i4) {
									n7--;
									n4++;
								}

								if (i7 < i3) {
									n7--;
									n3++;
								}

								if (i7 < i2) {
									n7--;
									n2++;
								}

								if (i7 < i1) {
									n7--;
									n1++;
								}



								cardsList[n1] = new Card(v1, s1);
								cardsList[n2] = new Card(v2, s2);
								cardsList[n3] = new Card(v3, s3);
								cardsList[n4] = new Card(v4, s4);
								cardsList[n5] = new Card((ushort)(i5 / 4 + 1), (ushort)(i5 % 4 + 1));
								cardsList[n6] = new Card(v6, s6);
								cardsList[n7] = new Card(v7, s7);
								var comb = new Combination(cardsList);
								var energy = comb.GetCachedEnergy();
								average += energy;
								distribution.Total++;
								if (energy >= Const.STREET_FLUSH_LOW7)
									streetFlushesItem.Count++;
								else if (energy >= Const.FOURS_LOW7)
									foursItem.Count++;
								else if (energy >= Const.FULL_HOUSE_LOW7)
									fullHousesItem.Count++;
								else if (energy >= Const.FLUSH_LOW7)
									flushesItem.Count++;
								else if (energy >= Const.STREET_LOW7)
									streetsItem.Count++;
								else if (energy >= Const.THREES_LOW7)
									threesItem.Count++;
								else if (energy >= Const.TWO_PAIRS_LOW7)
									twoPairsItem.Count++;
								else if (energy >= Const.PAIRS_LOW7)
									pairsItem.Count++;
								else if (energy >= Const.HIGHS_LOW7)
									highsItem.Count++;
								//}
								//}
							}
						}

					}
				}
				if (i1 % 4 == 0)
					OnNotify(string.Format("Готово на {0:p2}...", distribution.Total / (double)total));
			}
			distribution.Average = average / distribution.Total;
			return distribution;
		}
	}
}