using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCalc.Classes {
	public class Combination {

		public Card[] Cards { get; set; }
		public Combination(params Card[] cards) {
			Cards = cards;
		}

		public int GetEnergy() {
			var empty = Cards.Where(c => c.IsEmpty).ToList();
			if (!empty.Any())
				return getEnergy();
			var replacers = new List<Card>();

			return 0;
		}

		private static int[] suits = new int[4];

		public int GetCachedEnergy(bool sort = false){
			suits[0] = 0;
			suits[1] = 0;
			suits[2] = 0;
			suits[3] = 0;

			var cards = Cards;
			var flush = 0;

			for (int i = 0; i < cards.Length; i++) {
				var s = cards[i].Suit - 1;
				var suitCount = suits[s];
				if (suitCount > 3) {
					flush = s + 1;
					break;
				}
				suits[s] = suitCount + 1;
			}
			if (flush == 0) {
				//ushort v1 = 0, v2 = 0, v3 = 0, v4 = 0, v5 = 0, v6 = 0, v7 = 0; ;
				//for (int i = 0; i < Cards.Count; i++) {
				//	var v = Cards[i].Value;
				//	if (v > v7) {
				//		v1 = v2;
				//		v2 = v3;
				//		v3 = v4;
				//		v4 = v5;
				//		v5 = v6;
				//		v6 = v7;
				//		v7 = v;
				//	}
				//}
				if (sort) {
					var list = cards.ToList();
					list.Sort((c1, c2) => c1.Value.CompareTo(c2.Value));
					cards = list.ToArray();
				}
				//var n = CombinationRatings.d6Coefficients[Cards[0].Value - 1] +
				//		CombinationRatings.d5Coefficients[Cards[1].Value - 1] +
				//		CombinationRatings.d4Coefficients[Cards[2].Value - 1] +
				//		CombinationRatings.d3Coefficients[Cards[3].Value - 1] +
				//		CombinationRatings.d2Coefficients[Cards[4].Value - 1] +
				//		CombinationRatings.d1Coefficients[Cards[5].Value - 1] +
				//		(Cards[6].Value - 1);
				var n = CombinationRatings.d6 * (uint)(cards[0].Value - 1) +
						CombinationRatings.d5 * (uint)(cards[1].Value - 1) +
						CombinationRatings.d4 * (uint)(cards[2].Value - 1) +
						CombinationRatings.d3 * (uint)(cards[3].Value - 1) +
						CombinationRatings.d2 * (uint)(cards[4].Value - 1) +
						CombinationRatings.d1 * (uint)(cards[5].Value - 1) +
						(uint)(cards[6].Value - 1);
				//var nn = (uint) n;
				return CombinationRatings.NoFlushCombinations[n];
			}

			var sortedList = cards.ToList();
			sortedList.Sort((c1, c2) => {
				if ((c1.Suit == flush) == (c2.Suit == flush))
					return c1.Value.CompareTo(c2.Value);
				return c1.Suit == flush ? -1 : 1;
			});
			cards = sortedList.ToArray();

			var m = CombinationRatings.f6 * (cards[0].Value - 1) +
						CombinationRatings.f5 * (cards[1].Value - 1) +
						CombinationRatings.f4 * (cards[2].Value - 1) +
						CombinationRatings.f3 * (cards[3].Value - 1) +
						CombinationRatings.f2 * (cards[4].Value - 1) +
						CombinationRatings.f1 * ((cards[5].Value - 1) + (cards[5].Suit == flush ? 0 : 13)) +
						(long)((cards[6].Value - 1) + (cards[6].Suit == flush ? 0 : 13));
			//return 1;
			var mm = (uint) m;
			return CombinationRatings.FlushCombinations[mm];
		}

		private int getEnergy() {
			ushort straight = 0;
			var straightFlush = false;
			var suits = new ushort[4];
			var values = new ushort[13];
			var pairs = new List<ushort>();
			var threes = new List<ushort>();
			var fours = new List<ushort>();

			var size = Cards.Length;

			for (int i1 = 0; i1 < size; i1++) {
				var sequence = new Card[5];
				sequence[0] = Cards[i1];

				suits[sequence[0].Suit - 1]++;
				values[sequence[0].Value - 1]++;
				if (values[sequence[0].Value - 1] == 2) {
					pairs.Add(sequence[0].Value);
				} else if (values[sequence[0].Value - 1] == 3) {
					pairs.Remove(sequence[0].Value);
					threes.Add(sequence[0].Value);
				} else if (values[sequence[0].Value - 1] == 4) {
					threes.Remove(sequence[0].Value);
					fours.Add(sequence[0].Value);
				}

				for (int i2 = i1 + 1; i2 < size; i2++) {
					sequence[1] = Cards[i2];
					if (sequence[1].Value == sequence[0].Value)
						continue;
					for (int i3 = i2 + 1; i3 < size; i3++) {
						sequence[2] = Cards[i3];
						if (sequence[2].Value == sequence[0].Value ||
							sequence[2].Value == sequence[1].Value)
							continue;
						for (int i4 = i3 + 1; i4 < size; i4++) {
							sequence[3] = Cards[i4];
							if (sequence[3].Value == sequence[0].Value ||
								 sequence[3].Value == sequence[1].Value ||
								 sequence[3].Value == sequence[2].Value)
								continue;
							for (int i5 = i4 + 1; i5 < size; i5++) {
								sequence[4] = Cards[i5];
								if (sequence[4].Value == sequence[0].Value ||
									sequence[4].Value == sequence[1].Value ||
									sequence[4].Value == sequence[2].Value ||
									sequence[4].Value == sequence[3].Value)
									continue;

								var max = sequence.Max(c => c.Value);
								var min = sequence.Min(c => c.Value);
								if (max == 13
									&& min == 1) {
									max = sequence.Where(c => c.Value != 13).Max(c => c.Value);
									min = 0;
								}
								if (max - min == 4) {
									var suit = sequence[0].Suit;
									var thisStraightFlush = sequence.ToList().All(s => s.Suit == suit);
									if (thisStraightFlush && !straightFlush) {
										straight = max;
										straightFlush = true;
									} else if (thisStraightFlush == straightFlush)
										straight = Math.Max(straight, max);
								}
							}
						}
					}
				}
			}

			var flush = suits.ToList().FindIndex(s => s > 4) + 1;

			if (straightFlush) {
				return Const.STREET_FLUSH_LOW5 + straight - 4;
			}

			var vList = values.ToList();
			var four = vList.LastIndexOf(4);
			if (four >= 0) {
				var kicker = vList.FindLastIndex(v => v > 0 && v < 4);
				if (kicker > four) kicker--;
				return Const.FOURS_LOW5 + four * 12 + kicker;
			}
			threes.Sort();
			pairs.Sort();
			var three = threes.LastOrDefault() - 1;
			var two = threes.Union(pairs).LastOrDefault(p => p - 1 != three) - 1;
			if (three >= 0 && two >= 0) {
				if (two > three) two--;
				return Const.FULL_HOUSE_LOW5 + three * 12 + two;
			}
			if (flush > 0) {
				var high1 = Cards.Where(c => c.Suit == flush).Max(c => c.Value);
				var high2 = Cards.Where(c => c.Suit == flush && c.Value < high1).Max(c => c.Value);
				var high3 = Cards.Where(c => c.Suit == flush && c.Value < high2).Max(c => c.Value);
				var high4 = Cards.Where(c => c.Suit == flush && c.Value < high3).Max(c => c.Value);
				var high5 = Cards.Where(c => c.Suit == flush && c.Value < high4).Max(c => c.Value);
				//var high1 = vList.FindLastIndex(v => v > 0 && Cards.Any(c => c.Suit == flush && c.Value == vList.IndexOf(v) + 1));
				//var high2 = vList.FindLastIndex(v => v > 0 && Cards.Any(c => c.Suit == flush && c.Value == vList.IndexOf(v) + 1) && vList.IndexOf(v) < high1);
				//var high3 = vList.FindLastIndex(v => v > 0 && Cards.Any(c => c.Suit == flush && c.Value == vList.IndexOf(v) + 1) && vList.IndexOf(v) < high2);
				//var high4 = vList.FindLastIndex(v => v > 0 && Cards.Any(c => c.Suit == flush && c.Value == vList.IndexOf(v) + 1) && vList.IndexOf(v) < high3);
				//var high5 = vList.FindLastIndex(v => v > 0 && Cards.Any(c => c.Suit == flush && c.Value == vList.IndexOf(v) + 1) && vList.IndexOf(v) < high4);
				double f = high1 * CombinationRatings.d4 + high2 * CombinationRatings.d3 + high3 * CombinationRatings.d2 + high4 * CombinationRatings.d1 + high5;
				return Const.FLUSH_LOW5 + CombinationRatings.Flush[f];
				//return Const.FLUSH_LOW + CombinationRatings.Flush.FindIndex(
				//	a =>
				//	a[0] == high5 &&
				//	a[1] == high4 &&
				//	a[2] == high3 &&
				//	a[3] == high2 &&
				//	a[4] == high1);
			}
			if (straight > 0) {
				return Const.STREET_LOW5 + straight - 4;
			}
			if (three >= 0) {
				var kicker1 = Cards.Where(c => c.Value != three + 1).Max(c => c.Value) - 1;
				var kicker2 = Cards.Where(c => c.Value != three + 1 && c.Value < kicker1 + 1).Max(c => c.Value) - 1;
				if (kicker1 > three) kicker1--;
				if (kicker2 > three) kicker2--;

				var kickerRating = CombinationRatings.Twos.FindIndex(c => c[0] == kicker2 + 1 && c[1] == kicker1 + 1);

				return Const.THREES_LOW5 + three * 66 + kickerRating;
			} if (two >= 0) {
				var two2 = pairs.LastOrDefault(p => p - 1 != two) - 1;
				if (two2 >= 0) {
					var kicker = Cards.Where(c => c.Value != two + 1 && c.Value != two2 + 1).Max(c => c.Value) - 1;
					if (kicker > two) kicker--;
					if (kicker > two2) kicker--;
					var pairsRating = CombinationRatings.Twos.FindIndex(c => c[0] == two2 + 1 && c[1] == two + 1);
					return Const.TWO_PAIRS_LOW5 + pairsRating * 11 + kicker;
				}
				var kicker1 = Cards.Where(c => c.Value != two + 1).Max(c => c.Value) - 1;
				var kicker2 = Cards.Where(c => c.Value != two + 1 && c.Value < kicker1 + 1).Max(c => c.Value) - 1;
				var kicker3 = Cards.Where(c => c.Value != two + 1 && c.Value < kicker2 + 1).Max(c => c.Value) - 1;

				if (kicker1 > two) kicker1--;
				if (kicker2 > two) kicker2--;
				if (kicker3 > two) kicker3--;

				var kickerRating = CombinationRatings.Threes.FindIndex(c => c[0] == kicker3 + 1 && c[1] == kicker2 + 1 && c[2] == kicker1 + 1);

				return Const.PAIRS_LOW5 + two * 220 + kickerRating;
			}
			var card1 = Cards.Max(c => c.Value);
			var card2 = Cards.Where(c => c.Value < card1).Max(c => c.Value);
			var card3 = Cards.Where(c => c.Value < card2).Max(c => c.Value);
			var card4 = Cards.Where(c => c.Value < card3).Max(c => c.Value);
			var card5 = Cards.Where(c => c.Value < card4).Max(c => c.Value);
			//return Const.HIGHS_LOW + CombinationRatings.Flush.FindIndex(
			//		a =>
			//		a[0] == card5 &&
			//		a[1] == card4 &&
			//		a[2] == card3 &&
			//		a[3] == card2 &&
			//		a[4] == card1);

			double e = card1 * CombinationRatings.d4 + card2 * CombinationRatings.d3 + card3 * CombinationRatings.d2 + card4 * CombinationRatings.d1 + card5;
			return Const.HIGHS_LOW5 + CombinationRatings.Flush[e];

			//for (int i = 0; i < CombinationRatings.Flush.Count; i++){
			//	var c = CombinationRatings.Flush[i];
			//	if (c[0] == card5
			//		&& c[1] == card4
			//		&& c[2] == card3
			//		&& c[3] == card2
			//		&& c[4] == card1)
			//		return Const.HIGHS_LOW + i;
			//}
			//return 0;
		}
	}
}