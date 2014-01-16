using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using MyCalc.Classes;

namespace MyCalc.VM {
	public class TestVM : HeaderVM {

		private string label;
		private ObservableCollection<Stat> stats;
		private bool closing;

		public TestVM(Dispatcher dispatcher)
			: base(dispatcher) {
			dispatcher.ShutdownStarted += (sender, args) => closing = true;
			this.Header = "Test";
			this.CloseTab += (sender, args) => closing = true;
		}

		public string Label { get { return label; } set { label = value; OnPropertyChanged("Label"); } }

		public ObservableCollection<Stat> Stats { get { return stats; } set { stats = value; OnPropertyChanged("Stats"); } }

		public void SetLabel(string message) {
			if (closing) return;
			UIDispatcher.Invoke(new Action(() => Label = message));
		}

		public void setStats(List<Stat> stats) {
			stats.RemoveAll(i => i == null);
			stats.Sort((s1, s2) => {
				if (s1 == null && s2 == null) return 0;
				if (s1 == null) return 0.CompareTo(1);
				if (s2 == null) return 1.CompareTo(0);
				return s1.EnergyInt.CompareTo(s2.EnergyInt);
			});

			Stat.Total = stats.Sum(s => s == null ? 0 : s.CombinationsInt);
			var entropy = stats.Sum(
				s => {
					var p = s.CombinationsInt / (double)Stat.Total;
					return -p * Math.Log(p, 2);
				});
			SetLabel(string.Format("Всего комбинаций: {0:n0}. Энтропия: {1:n5} бит.", Stat.Total, entropy));
			Stats = new ObservableCollection<Stat>(stats);
		}

		private void addSpan(TimeSpan span) {
			SetLabel(Label + string.Format(" Заняло: {0:n3}", span.TotalSeconds));
		}

		public void CalculateStatsStreetFlush() {
			Stat.Total = 10;
			Stat.AverageWidth = 1000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i = 0; i < 10; i++) {
				var card1 = new Card((ushort)(i == 0 ? 13 : i), 1);
				var card2 = new Card((ushort)(i + 1), 1);
				var card3 = new Card((ushort)(i + 2), 1);
				var card4 = new Card((ushort)(i + 3), 1);
				var card5 = new Card((ushort)(i + 4), 1);
				//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
				//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
				var comb = new Combination(card1, card2, card3, card4, card5/*, card6, card7*/);
				var energy = comb.GetEnergy();
				var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
				if (exist != null) {
					exist.CombinationsInt++;
				} else {
					list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
				}
			}

			setStats(list);
			SetLabel(string.Format("Всего комбинаций: {0:n0}", list.Sum(s => s.CombinationsInt)));
		}

		public void CalculateStatsFours() {

			Stat.AverageWidth = 10000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i1 = 0; i1 < 13; i1++) {
				for (int i2 = 0; i2 < 52; i2++) {
					if (i2 % 13 == i1) continue;
					var card1 = new Card((ushort)(i1 + 1), 1);
					var card2 = new Card((ushort)(i1 + 1), 2);
					var card3 = new Card((ushort)(i1 + 1), 3);
					var card4 = new Card((ushort)(i1 + 1), 4);
					var card5 = new Card((ushort)(i2 % 13 + 1), (ushort)(i2 / 13 + 1));
					//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
					//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
					var comb = new Combination(card1, card2, card3, card4, card5 /*, card6, card7*/);
					var energy = comb.GetEnergy();
					var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
					if (exist != null) {
						exist.CombinationsInt++;
					} else {
						list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
					}
				}
			}
			Stat.Total = list.Sum(s => s.CombinationsInt);
			setStats(list);
			SetLabel(string.Format("Всего комбинаций: {0:n0}", Stat.Total));
		}

		public void CalculateStatsFullHouses() {

			Stat.AverageWidth = 10000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i1 = 0; i1 < 13; i1++) {

				for (int i2 = 0; i2 < 13; i2++) {

					if (i2 == i1) continue;
					var card1 = new Card((ushort)(i1 + 1), 1);
					var card2 = new Card((ushort)(i1 + 1), 2);
					var card3 = new Card((ushort)(i1 + 1), 3);
					var card4 = new Card((ushort)(i2 + 1), 4);
					var card5 = new Card((ushort)(i2 + 1), 3);
					//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
					//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
					var comb = new Combination(card1, card2, card3, card4, card5 /*, card6, card7*/);
					var energy = comb.GetEnergy();
					var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
					if (exist != null) {
						exist.CombinationsInt++;
					} else {
						list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
					}
				}
			}
			Stat.Total = list.Sum(s => s.CombinationsInt);
			setStats(list);
			SetLabel(string.Format("Всего комбинаций: {0:n0}", Stat.Total));
		}

		public void CalculateStatsFlushes() {

			Stat.AverageWidth = 20000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i1 = 0; i1 < 13; i1++) {
				for (int i2 = i1 + 1; i2 < 13; i2++) {
					for (int i3 = i2 + 1; i3 < 13; i3++) {
						for (int i4 = i3 + 1; i4 < 13; i4++) {
							for (int i5 = i4 + 1; i5 < 13; i5++) {

								var card1 = new Card((ushort)(i1 + 1), 1);
								var card2 = new Card((ushort)(i2 + 1), 1);
								var card3 = new Card((ushort)(i3 + 1), 1);
								var card4 = new Card((ushort)(i4 + 1), 1);
								var card5 = new Card((ushort)(i5 + 1), 1);
								//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
								//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
								var comb = new Combination(card1, card2, card3, card4, card5 /*, card6, card7*/);
								var energy = comb.GetEnergy();
								var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
								if (exist != null) {
									exist.CombinationsInt++;
								} else {
									list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
								}
							}
						}
					}
				}
				SetLabel(string.Format("Обработано {0} ...", i1/*, i2, i3, i4, i5, i6, i7*/));
			}
			Stat.Total = list.Sum(s => s.CombinationsInt);
			setStats(list);
			SetLabel(string.Format("Всего комбинаций: {0:n0}", Stat.Total));
		}

		public void CalculateStatsStreet() {
			Stat.Total = 10;
			Stat.AverageWidth = 1000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i = 0; i < 10; i++) {
				var card1 = new Card((ushort)(i == 0 ? 13 : i), 1);
				var card2 = new Card((ushort)(i + 1), 2);
				var card3 = new Card((ushort)(i + 2), 3);
				var card4 = new Card((ushort)(i + 3), 4);
				var card5 = new Card((ushort)(i + 4), 1);
				//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
				//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
				var comb = new Combination(card1, card2, card3, card4, card5/*, card6, card7*/);
				var energy = comb.GetEnergy();
				var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
				if (exist != null) {
					exist.CombinationsInt++;
				} else {
					list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
				}
			}

			setStats(list);
			SetLabel(string.Format("Всего комбинаций: {0:n0}", list.Sum(s => s.CombinationsInt)));
		}

		public void CalculateStatsThrees() {

			Stat.AverageWidth = 10000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i1 = 0; i1 < 13; i1++) {
				for (int i2 = 0; i2 < 52; i2++) {
					if (i2 % 13 == i1) continue;
					for (int i3 = 0; i3 < 52; i3++) {
						if (i3 % 13 == i1 || i3 % 13 == i2 % 13) continue;
						var card1 = new Card((ushort)(i1 + 1), 1);
						var card2 = new Card((ushort)(i1 + 1), 2);
						var card3 = new Card((ushort)(i1 + 1), 3);
						var card4 = new Card((ushort)(i2 % 13 + 1), (ushort)(i2 / 13 + 1));
						var card5 = new Card((ushort)(i3 % 13 + 1), (ushort)(i3 / 13 + 1));
						//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
						//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
						var comb = new Combination(card1, card2, card3, card4, card5 /*, card6, card7*/);
						var energy = comb.GetEnergy();
						var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
						if (exist != null) {
							exist.CombinationsInt++;
						} else {
							list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
						}
					}
				}
			}
			Stat.Total = list.Sum(s => s.CombinationsInt);
			setStats(list);
			SetLabel(string.Format("Всего комбинаций: {0:n0}", Stat.Total));
		}

		public void CalculateStatsTwoPairs() {

			Stat.AverageWidth = 10000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i1 = 0; i1 < 13; i1++) {
				for (int i2 = i1 + 1; i2 < 13; i2++) {
					for (int i3 = 0; i3 < 52; i3++) {
						if (i3 % 13 == i1 || i3 % 13 == i2) continue;
						var card1 = new Card((ushort)(i1 + 1), 1);
						var card2 = new Card((ushort)(i1 + 1), 2);
						var card3 = new Card((ushort)(i2 + 1), 3);
						var card4 = new Card((ushort)(i2 + 1), 2);
						var card5 = new Card((ushort)(i3 % 13 + 1), (ushort)(i3 / 13 + 1));
						//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
						//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
						var comb = new Combination(card1, card2, card3, card4, card5 /*, card6, card7*/);
						var energy = comb.GetEnergy();
						var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
						if (exist != null) {
							exist.CombinationsInt++;
						} else {
							list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
						}
					}
				}
			}

			setStats(list);

		}

		public void CalculateStatsPairs() {

			Stat.AverageWidth = 10000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i1 = 0; i1 < 13; i1++) {
				for (int i2 = 0; i2 < 52; i2++) {
					if (i2 % 13 == i1) continue;
					for (int i3 = 0; i3 < 52; i3++) {
						if (i3 % 13 == i1 || i3 % 13 == i2 % 13) continue;
						for (int i4 = 0; i4 < 52; i4++) {
							if (i4 % 13 == i1 || i4 % 13 == i2 % 13 || i4 % 13 == i3 % 13) continue;
							var card1 = new Card((ushort)(i1 + 1), 1);
							var card2 = new Card((ushort)(i1 + 1), 2);
							var card3 = new Card((ushort)(i2 % 13 + 1), (ushort)(i3 / 13 + 1));
							var card4 = new Card((ushort)(i3 % 13 + 1), (ushort)(i3 / 13 + 1));
							var card5 = new Card((ushort)(i4 % 13 + 1), (ushort)(i4 / 13 + 1));
							//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
							//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
							var comb = new Combination(card1, card2, card3, card4, card5 /*, card6, card7*/);
							var energy = comb.GetEnergy();
							var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
							if (exist != null) {
								exist.CombinationsInt++;
							} else {
								list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
							}
						}
					}
					SetLabel(string.Format("Обработано {0}, {1} ...", i1, i2/*, i3, i4, i5, i6, i7*/));
				}
			}
			Stat.Total = list.Sum(s => s.CombinationsInt);
			setStats(list);
			SetLabel(string.Format("Всего комбинаций: {0:n0}", Stat.Total));
		}

		public void CalculateStatsHighs() {

			Stat.AverageWidth = 10000;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>();

			for (int i1 = 0; i1 < 52; i1++) {
				for (int i2 = i1 + 1; i2 < 52; i2++) {
					if (i2 % 13 == i1 % 13) continue;
					for (int i3 = i2 + 1; i3 < 52; i3++) {
						if (i3 % 13 == i1 % 13 || i3 % 13 == i2 % 13) continue;
						for (int i4 = i3 + 1; i4 < 52; i4++) {
							if (i4 % 13 == i1 % 13 || i4 % 13 == i2 % 13 || i4 % 13 == i3 % 13) continue;
							for (int i5 = i4 + 1; i5 < 52; i5++) {
								if (i5 % 13 == i1 % 13 || i5 % 13 == i2 % 13 || i5 % 13 == i3 % 13 || i5 % 13 == i4 % 13) continue;
								var card1 = new Card((ushort)(i1 % 13 + 1), (ushort)(i1 / 13 + 1));
								var card2 = new Card((ushort)(i2 % 13 + 1), (ushort)(i2 / 13 + 1));
								var card3 = new Card((ushort)(i3 % 13 + 1), (ushort)(i3 / 13 + 1));
								var card4 = new Card((ushort)(i4 % 13 + 1), (ushort)(i4 / 13 + 1));
								var card5 = new Card((ushort)(i5 % 13 + 1), (ushort)(i5 / 13 + 1));
								//var card6 = new Card((ushort)(i6 % 13 + 1), (ushort)(i6 / 13 + 1));
								//var card7 = new Card((ushort)(i7 % 13 + 1), (ushort)(i7 / 13 + 1));
								var comb = new Combination(card1, card2, card3, card4, card5 /*, card6, card7*/);
								var energy = comb.GetEnergy();
								var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
								if (exist != null) {
									exist.CombinationsInt++;
								} else {
									list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
								}
							}
						}
					}
					SetLabel(string.Format("Обработано {0}, {1} ...", i1, i2/*, i3, i4, i5, i6, i7*/));
				}
			}
			Stat.Total = list.Sum(s => s.CombinationsInt);
			setStats(list);
			SetLabel(string.Format("Всего комбинаций: {0:n0}", Stat.Total));
		}

		public void CalculateStats5() {
			var start = DateTime.Now;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>(new Stat[Const.STREET_FLUSH_HIGH5]);
			var deck = 52;
			for (int i1 = 0; i1 < deck; i1++) {
				for (int i2 = i1 + 1; i2 < deck; i2++) {
					for (int i3 = i2 + 1; i3 < deck; i3++) {
						for (int i4 = i3 + 1; i4 < deck; i4++) {
							Parallel.For(i4 + 1, deck, (i5) => {
								//for (int i5 = i4 + 1; i5 < deck; i5++) {

								var card1 = new Card((ushort)(i1 % 13 + 1), (ushort)(i1 / 13 + 1));
								var card2 = new Card((ushort)(i2 % 13 + 1), (ushort)(i2 / 13 + 1));
								var card3 = new Card((ushort)(i3 % 13 + 1), (ushort)(i3 / 13 + 1));
								var card4 = new Card((ushort)(i4 % 13 + 1), (ushort)(i4 / 13 + 1));
								var card5 = new Card((ushort)(i5 % 13 + 1), (ushort)(i5 / 13 + 1));

								var comb = new Combination(card1, card2, card3, card4, card5 /*, card6, card7*/);
								var energy = comb.GetEnergy();
								Stat exist;
								if (list[energy - 1] == null) {
									exist = new Stat { CombinationsInt = 1, EnergyInt = energy };
									list[energy - 1] = exist;
								} else {
									exist = list[energy - 1];
									exist.CombinationsInt++;
								}
								//Stat exist;
								//lock (list) {
								//	exist = list.FirstOrDefault(s => s.EnergyInt == energy);
								//}
								//if (exist != null) {
								//	exist.CombinationsInt++;
								//} else {
								//	lock (list) {
								//		list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
								//	}
								//}
								//	}
								//}
							});
						}
					}
					SetLabel(string.Format("Обработано {0}, {1} ...", i1, i2/*, i3, i4, i5, i6, i7*/));
				}
			}
			setStats(list);
			addSpan(DateTime.Now - start);
		}


		public void CalculateStatsNoFlush(bool save = false) {
			var helper = new DBHelper(Properties.Settings.Default.MainConnectionString);
			var start = DateTime.Now;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>(new Stat[Const.STREET_FLUSH_HIGH5]);
			var deck = 13;
			var valueCounts = new int[13];
			for (int i1 = 0; i1 < deck; i1++) {
				valueCounts[i1]++;
				for (int i2 = i1; i2 < deck; i2++) {
					valueCounts[i2]++;
					for (int i3 = i2; i3 < deck; i3++) {
						valueCounts[i3]++;
						for (int i4 = i3; i4 < deck; i4++) {
							valueCounts[i4]++;
							for (int i5 = i4; i5 < deck; i5++) {
								valueCounts[i5]++;
								if (valueCounts[i5] > 4) {
									valueCounts[i5]--;
									continue;
								}
								for (int i6 = i5; i6 < deck; i6++) {
									valueCounts[i6]++;
									if (valueCounts[i6] > 4) {
										valueCounts[i6]--;
										continue;
									}
									for (int i7 = i6; i7 < deck; i7++) {
										valueCounts[i7]++;
										if (valueCounts[i7] > 4) {
											valueCounts[i7]--;
											continue;
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
										Stat exist;
										if (list[energy - 1] == null) {
											exist = new Stat { CombinationsInt = 1, EnergyInt = energy };
											list[energy - 1] = exist;
										} else {
											exist = list[energy - 1];
											exist.CombinationsInt++;
										}
										if (save) {
											helper.SaveCombinationEnergyNoFlush(i1, i2, i3, i4, i5, i6, i7, energy);
										}
										valueCounts[i7]--;
									}
									valueCounts[i6]--;
								}
								valueCounts[i5]--;
							}
							valueCounts[i4]--;
						}
						SetLabel(string.Format("Обработано {0}, {1}, {2} ...", i1, i2, i3/*, i4, i5, i6, i7*/));
						addSpan(DateTime.Now - start);
						valueCounts[i3]--;
					}
					valueCounts[i2]--;
				}
				valueCounts[i1]--;
			}
			setStats(list);
			addSpan(DateTime.Now - start);
		}

		public void CalculateStats7(bool save = false) {
			var helper = new DBHelper(Properties.Settings.Default.MainConnectionString);
			var start = DateTime.Now;
			SetLabel("Starting calculating stats...");
			var list = new List<Stat>(new Stat[Const.STREET_FLUSH_HIGH5]);
			var deck = 52;
			for (int i1 = 0; i1 < deck; i1++) {
				//if (i1 < 4) continue;
				for (int i2 = i1 + 1; i2 < deck; i2++) {
					//if (i1 == 4 && i2 < 9) continue;
					for (int i3 = i2 + 1; i3 < deck; i3++) {
						//if (i1 == 4 && i2 == 9 && i3 < 22) continue;
						for (int i4 = i3 + 1; i4 < deck; i4++) {
							//if (i1 == 4 && i2 == 9 && i3 == 22 && i4 < 41) continue;
							for (int i5 = i4 + 1; i5 < deck; i5++) {
								//if (i1 == 4 && i2 == 9 && i3 == 22 && i4 == 41 && i5 < 42) continue;
								for (int i6 = i5 + 1; i6 < deck; i6++) {
									if (closing) return;
									//if (i1 == 4 && i2 == 9 && i3 == 22 && i4 == 41 && i5 == 42 && i6 < 47) continue;
									//Parallel.For(i6 + 1, deck, i7 => {
										for (int i7 = i6 + 1; i7 < deck; i7++) {
										//if (i1 == 4 && i2 == 9 && i3 == 22 && i4 == 41 && i5 == 42 && i6 == 47 && i7 < 49) return;
										var card1 = new Card((ushort)(i1 / 4 + 1), (ushort)(i1 % 4 + 1));
										var card2 = new Card((ushort)(i2 / 4 + 1), (ushort)(i2 % 4 + 1));
										var card3 = new Card((ushort)(i3 / 4 + 1), (ushort)(i3 % 4 + 1));
										var card4 = new Card((ushort)(i4 / 4 + 1), (ushort)(i4 % 4 + 1));
										var card5 = new Card((ushort)(i5 / 4 + 1), (ushort)(i5 % 4 + 1));
										var card6 = new Card((ushort)(i6 / 4 + 1), (ushort)(i6 % 4 + 1));
										var card7 = new Card((ushort)(i7 / 4 + 1), (ushort)(i7 % 4 + 1));
										var comb = new Combination(card1, card2, card3, card4, card5, card6, card7);
										var energy = comb.GetCachedEnergy();
										//if (energy == 7462)
										//	Debug.WriteLine(
										//		string.Format(
										//			"Energy={0} c1=({1},{2}) c2=({3},{4}) c3=({5},{6}) c4=({7},{8}) c5=({9},{10}) c6=({11},{12}) c7=({13},{14})",
										//			energy,
										//			card1.Value,
										//			card1.Suit,
										//			card2.Value,
										//			card2.Suit,
										//			card3.Value,
										//			card3.Suit,
										//			card4.Value,
										//			card4.Suit,
										//			card5.Value,
										//			card5.Suit,
										//			card6.Value,
										//			card6.Suit,
										//			card7.Value,
										//			card7.Suit));
										Stat exist;
										if (list[energy - 1] == null) {
											exist = new Stat { CombinationsInt = 1, EnergyInt = energy };
											list[energy - 1] = exist;
										} else {
											exist = list[energy - 1];
											exist.CombinationsInt++;
										}
										//var exist = list.FirstOrDefault(s => s.EnergyInt == energy);
										//if (exist != null) {
										//	exist.CombinationsInt++;
										//} else {
										//	list.Add(new Stat { CombinationsInt = 1, EnergyInt = energy });
										//}
										//}
										if (save) {
											helper.SaveCombinationEnergy(i1, i2, i3, i4, i5, i6, i7, energy);
										}
									//});
									}
								}
							}
						}
					}
					SetLabel(string.Format("Обработано {0}, {1}...", i1, i2/*, i3, i4, i5, i6, i7*/));
					addSpan(DateTime.Now - start);
				}
			}


			setStats(list);
			addSpan(DateTime.Now - start);
		}
	}
}