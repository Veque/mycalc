using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyCalc.Properties;
using MyCalc.VM;

namespace MyCalc.Classes {
	public static class CombinationRatings {
		static CombinationRatings() {
			Flush = new Dictionary<double, int>();
			Threes = new List<ushort[]>();
			Twos = new List<ushort[]>();
			Conversion5to7 = new Dictionary<int, int>();
			NoFlushCombinations = new Dictionary<uint, ushort>();
			FlushCombinations = new Dictionary<uint, ushort>();

			d1 = 13;
			d2 = d1 * 13;
			d3 = d2 * 13;
			d4 = d3 * 13;
			d5 = d4 * 13;
			d6 = d5 * 13;

			f1 = 26;
			f2 = f1 * 26;
			f3 = f2 * 13;
			f4 = f3 * 13;
			f5 = f4 * 13;
			f6 = f5 * 13;

			d1Coefficients = new uint[52];
			d2Coefficients = new uint[52];
			d3Coefficients = new uint[52];
			d4Coefficients = new uint[52];
			d5Coefficients = new uint[52];
			d6Coefficients = new uint[52];

			for (uint i = 0; i < 52; i++) {
				d1Coefficients[i] = i * d1;
				d2Coefficients[i] = i * d2;
				d3Coefficients[i] = i * d3;
				d4Coefficients[i] = i * d4;
				d5Coefficients[i] = i * d5;
				d6Coefficients[i] = i * d6;
			}
		}

		public static uint[] d1Coefficients { get; set; }
		public static uint[] d2Coefficients { get; set; }
		public static uint[] d3Coefficients { get; set; }
		public static uint[] d4Coefficients { get; set; }
		public static uint[] d5Coefficients { get; set; }
		public static uint[] d6Coefficients { get; set; }

		public static Dictionary<double, int> Flush { get; set; }
		public static Dictionary<uint, ushort> NoFlushCombinations { get; set; }
		public static Dictionary<uint, ushort> FlushCombinations { get; set; }
		public static Dictionary<int, int> Conversion5to7 { get; set; }
		public static List<ushort[]> Threes { get; set; }
		public static List<ushort[]> Twos { get; set; }
		public static uint d1;
		public static uint d2;
		public static uint d3;
		public static uint d4;
		public static uint d5;
		public static uint d6;

		public static uint f1;
		public static uint f2;
		public static uint f3;
		public static uint f4;
		public static uint f5;
		public static uint f6;


		public static void InitFlushRating() {
			Flush = new Dictionary<double, int>();
			var tmp = new List<ushort[]>();
			for (ushort i1 = 0; i1 < 13; i1++) {
				for (var i2 = (ushort)(i1 + 1); i2 < 13; i2++) {
					for (var i3 = (ushort)(i2 + 1); i3 < 13; i3++) {
						for (var i4 = (ushort)(i3 + 1); i4 < 13; i4++) {
							for (var i5 = (ushort)(i4 + 1); i5 < 13; i5++) {
								if (i5 - i1 == 4 ||
									i5 == 12 && i1 == 0 && i2 == 1 && i3 == 2 && i4 == 3) continue;
								tmp.Add(new[]{
									(ushort)(i1 + 1), 
									(ushort)(i2 + 1),
									(ushort)(i3 + 1),
									(ushort)(i4 + 1),
									(ushort)(i5 + 1)
								});
							}
						}
					}
				}
			}
			var d = 13;
			tmp.Sort((c1, c2) => {
				var r1 = c1[4] * Math.Pow(d, 4) + c1[3] * Math.Pow(d, 3) + c1[2] * Math.Pow(d, 2) + c1[1] * d + c1[0];
				var r2 = c2[4] * Math.Pow(d, 4) + c2[3] * Math.Pow(d, 3) + c2[2] * Math.Pow(d, 2) + c2[1] * d + c2[0];
				return r1.CompareTo(r2);
			});

			for (int i = 0; i < tmp.Count; i++) {
				var c = tmp[i];
				double e = c[4] * Math.Pow(d, 4) + c[3] * Math.Pow(d, 3) + c[2] * Math.Pow(d, 2) + c[1] * d + c[0];
				Flush[e] = i;
			}
		}

		public static void InitConversion() {
			var db = new DBHelper(Settings.Default.MainConnectionString);
			Conversion5to7 = db.GetConversion();
		}

		public static void InitThreesRating() {
			for (ushort i1 = 0; i1 < 13; i1++) {
				for (var i2 = (ushort)(i1 + 1); i2 < 13; i2++) {
					for (var i3 = (ushort)(i2 + 1); i3 < 13; i3++) {
						Threes.Add(new[]{
									(ushort)(i1 + 1), 
									(ushort)(i2 + 1),
									(ushort)(i3 + 1),
								});
					}
				}
			}
			Threes.Sort((c1, c2) => {
				var d = 13;
				var r1 = c1[2] * Math.Pow(d, 2) + c1[1] * d + c1[0];
				var r2 = c2[2] * Math.Pow(d, 2) + c2[1] * d + c2[0];
				return r1.CompareTo(r2);
			});
		}

		public static void InitTwosRating() {
			for (ushort i1 = 0; i1 < 13; i1++) {
				for (var i2 = (ushort)(i1 + 1); i2 < 13; i2++) {
					Twos.Add(new[]{
									(ushort)(i1 + 1), 
									(ushort)(i2 + 1),
								});
				}
			}
			Twos.Sort((c1, c2) => {
				var d = 13;
				var r1 = c1[1] * d + c1[0];
				var r2 = c2[1] * d + c2[0];
				return r1.CompareTo(r2);
			});
		}

		public static void InitNoFlushCombinationsRating() {
			var deck = 13;
			var valueCounts = new int[13];
			for (uint i1 = 0; i1 < deck; i1++) {
				valueCounts[i1]++;
				var n1 = i1 * d6;
				for (uint i2 = i1; i2 < deck; i2++) {
					valueCounts[i2]++;
					var n2 = i2 * d5;
					for (uint i3 = i2; i3 < deck; i3++) {
						valueCounts[i3]++;
						var n3 = i3 * d4;
						for (uint i4 = i3; i4 < deck; i4++) {
							valueCounts[i4]++;
							var n4 = i4 * d3;
							for (uint i5 = i4; i5 < deck; i5++) {
								valueCounts[i5]++;
								if (valueCounts[i5] > 4) {
									valueCounts[i5]--;
									continue;
								}
								var n5 = i5 * d2;
								for (uint i6 = i5; i6 < deck; i6++) {
									valueCounts[i6]++;
									if (valueCounts[i6] > 4) {
										valueCounts[i6]--;
										continue;
									}
									var n6 = i6 * d1;
									Parallel.For((int)i6, deck, new Action<int>(i7 => {
										if (valueCounts[i7] + 1 > 4) {
											return;
										}
										var card1 = new Card((ushort)(i1 + 1), 1);
										var card2 = new Card((ushort)(i2 + 1), 2);
										var card3 = new Card((ushort)(i3 + 1), 3);
										var card4 = new Card((ushort)(i4 + 1), 4);
										var card5 = new Card((ushort)(i5 + 1), 1);
										var card6 = new Card((ushort)(i6 + 1), 2);
										var card7 = new Card((ushort)(i7 + 1), 3);
										var comb = new Combination(card1, card2, card3, card4, card5, card6, card7);
										var energy = comb.GetEnergy();
										var n = n1 + n2 + n3 + n4 + n5 + n6 + (uint)i7;
										lock (NoFlushCombinations) {
											NoFlushCombinations[n] = (ushort)Conversion5to7[energy];
										}

										//Stat exist;
										//if (list[energy - 1] == null) {
										//	exist = new Stat { CombinationsInt = 1, EnergyInt = energy };
										//	list[energy - 1] = exist;
										//} else {
										//	exist = list[energy - 1];
										//	exist.CombinationsInt++;
										//}
										//if (save) {
										//	helper.SaveCombinationEnergyNoFlush(i1, i2, i3, i4, i5, i6, i7, energy);
										//}
									}));
									valueCounts[i6]--;
								}
								valueCounts[i5]--;
							}
							valueCounts[i4]--;
						}
						valueCounts[i3]--;
					}
					valueCounts[i2]--;
				}
				valueCounts[i1]--;
			}
		}

		public static void InitFlushCombinationsRating() {
			var deck = 13;
			var valueCounts = new int[13];
			for (uint i1 = 0; i1 < deck; i1++) {
				valueCounts[i1]++;
				var n1 = i1 * f6;
				for (uint i2 = i1 + 1; i2 < deck; i2++) {
					valueCounts[i2]++;
					var n2 = i2 * f5;
					for (uint i3 = i2 + 1; i3 < deck; i3++) {
						valueCounts[i3]++;
						var n3 = i3 * f4;
						for (uint i4 = i3 + 1; i4 < deck; i4++) {
							valueCounts[i4]++;
							var n4 = i4 * f3;
							for (uint i5 = i4 + 1; i5 < deck; i5++) {
								valueCounts[i5]++;
								var n5 = i5 * f2;
								for (uint s1 = 0; s1 < 2; s1++) {
									for (uint i6 = s1 == 0 ? i5 + 1 : 0; i6 < deck; i6++) {
										valueCounts[i6]++;
										var n6 = (i6 + s1 * 13) * f1;
										for (uint s2 = s1; s2 < 2; s2++) {
											Parallel.For((int)(s2 == 0 ? i6 + 1 : (s1 == 0 ? 0 : i6)), deck, new Action<int>(i7 => {
												//for (int i7 = s2 == 0 ? i6 + 1 : 0; i7 < deck; i7++) {
												var card1 = new Card((ushort)(i1 + 1), 1);
												var card2 = new Card((ushort)(i2 + 1), 1);
												var card3 = new Card((ushort)(i3 + 1), 1);
												var card4 = new Card((ushort)(i4 + 1), 1);
												var card5 = new Card((ushort)(i5 + 1), 1);
												var card6 = new Card((ushort)(i6 + 1), (ushort)(s1 + 1));
												var card7 = new Card((ushort)(i7 + 1), (ushort)(s2 + 1));
												var comb = new Combination(card1, card2, card3, card4, card5, card6, card7);
												var energy = comb.GetEnergy();
												var n = n1 + n2 + n3 + n4 + n5 + n6 + (uint)i7 + s2 * 13;
												lock (FlushCombinations) {
													FlushCombinations[n] = (ushort)Conversion5to7[energy];
												}

												//Stat exist;
												//if (list[energy - 1] == null) {
												//	exist = new Stat { CombinationsInt = 1, EnergyInt = energy };
												//	list[energy - 1] = exist;
												//} else {
												//	exist = list[energy - 1];
												//	exist.CombinationsInt++;
												//}
												//if (save) {
												//	helper.SaveCombinationEnergyNoFlush(i1, i2, i3, i4, i5, i6, i7, energy);
												//}
											}));
											//}
											valueCounts[i6]--;
										}
									}
								}
								valueCounts[i5]--;
							}
							valueCounts[i4]--;
						}
						valueCounts[i3]--;
					}
					valueCounts[i2]--;
				}
				valueCounts[i1]--;
			}
		}
	}
}