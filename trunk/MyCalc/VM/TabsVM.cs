using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MVVM;
using MyCalc.Classes;
using MyCalc.UI;
using SplashWindow = MVVM.SplashWindow;

namespace MyCalc.VM {
	public class TabsVM : ViewModelBase {

		public TabsVM(Dispatcher disptcher)
			: base(disptcher) {
			tabs = new ObservableCollection<HeaderVM>();
			using (new Splash<SplashWindow>()) {
				CombinationRatings.InitFlushRating();
				CombinationRatings.InitThreesRating();
				CombinationRatings.InitTwosRating();
				CombinationRatings.InitConversion();
				CombinationRatings.InitNoFlushCombinationsRating();
				CombinationRatings.InitFlushCombinationsRating();
				CardResources.Init();
			}
		}

		private HeaderVM selectedTab;

		public HeaderVM SelectedTab {
			get { return selectedTab; }
			set {
				selectedTab = value;
				OnPropertiesChanged("SelectedTab");
			}
		}

		private ObservableCollection<HeaderVM> tabs;

		public ObservableCollection<HeaderVM> Tabs {
			get { return tabs; }
			set {
				tabs = value;
				OnPropertiesChanged("Tabs");
			}
		}

		#region Commands
		public Command ExitCommand {
			get {
				return new Command(s => Application.Current.MainWindow.Close());
			}
		}
		public Command TestCommand {
			get {
				return new Command(s => {
					var vm = new TestVM(UIDispatcher);
					vm.CloseTab += (sender, args) => Tabs.Remove(sender as HeaderVM);
					Tabs.Add(vm);
					SelectedTab = vm;
					new Thread(() => vm.CalculateStats7()).Start();
				});
			}
		}
		public Command CombinationsDistribution {
			get {
				return new Command(s => {
					var vm = new DistributionVM(UIDispatcher) {
						Header = "Комбинации",
						WidthPerDot = 0.2,
						MaxHeight = 500
					};
					vm.CloseTab += (sender, args) => Tabs.Remove(vm);
					Tabs.Add(vm);
					SelectedTab = vm;
					var thread = new Thread(
						() => {
							vm.ShowOverlay = true;
							try {
								var calculator = new DistributionCalculator();
								calculator.Notify += (sender, e) => vm.OverlayText = e.Message;
								var distr = calculator.CalculateCombinationsDistribution();
								vm.Distribution = distr;
							} finally {
								vm.ShowOverlay = false;
							}
						});
					thread.Start();

				});
			}
		}
		public Command PairDistribution {
			get {
				return new Command(s => {
					var vm = new DistributionVM(UIDispatcher) {
						Header = "Пара",
						WidthPerDot = 0.2,
						MaxHeight = 500
					};
					vm.CloseTab += (sender, args) => Tabs.Remove(vm);
					Tabs.Add(vm);
					SelectedTab = vm;
					var thread = new Thread(
						() => {
							vm.ShowOverlay = true;
							try {
								var calculator = new DistributionCalculator();
								calculator.Notify += (sender, e) => vm.OverlayText = e.Message;
								var distr = calculator.CalculateDistributionForPair(1, 1, 1, 2);
								vm.Distribution = distr;
							} finally {
								vm.ShowOverlay = false;
							}
						});
					thread.Start();

				});
			}
		}

		public Command ChooseCardCommand {
			get {
				return new Command(s => {
					var card = CardChooser.ChooseCard(UIDispatcher);
					card.ToString();
				});
			}
		}

		public Command ShowDistributionPanelCommand {
			get {
				return new Command(s => {
					var vm = new CardListDistributionVM(UIDispatcher);
					vm.CloseTab += (sender, e) => Tabs.Remove(vm);
					Tabs.Add(vm);
					SelectedTab = vm;
				});
			}
		}
		#endregion
	}
}