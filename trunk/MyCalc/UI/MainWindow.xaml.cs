using System;
using System.Threading;
using System.Windows;
using MyCalc.Classes;
using MyCalc.Properties;
using MyCalc.VM;

namespace MyCalc.UI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			//context.UIDispatcher = this.Dispatcher;
			DataContext = new TabsVM(this.Dispatcher);
			//this.Loaded += (sender, args) =>{
			//	//var helper = new DBHelper(Settings.Default.MainConnectionString);
			//	//var energies = helper.GetEnergies();
			//	//energies.ToString();
			//	//MessageBox.Show(energies.Count.ToString(), "!!!");
			//	//var start = DateTime.Now;
			//	CombinationRatings.InitFlushRating();
			//	CombinationRatings.InitThreesRating();
			//	CombinationRatings.InitTwosRating();
			//	CombinationRatings.InitConversion();
			//	CombinationRatings.InitNoFlushCombinationsRating();
			//	CombinationRatings.InitFlushCombinationsRating();
			//	//MessageBox.Show((DateTime.Now - start).TotalSeconds.ToString("n3"));
			//	new Thread(() => context.CalculateStats7(false)).Start();
			//};
		}
	}
}
