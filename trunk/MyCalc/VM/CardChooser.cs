using System.Windows;
using System.Windows.Threading;
using MyCalc.UI;

namespace MyCalc.VM{
	public class CardChooser{

		private static CardChooserVM chooser;

		public static CardVM ChooseCard(Dispatcher dispatcher){
			if(chooser == null)
				chooser = new CardChooserVM(dispatcher);

			var window = new TemplateWindow{
				Title = "Выбор карты",
				WindowStartupLocation = WindowStartupLocation.CenterScreen,
				WindowStyle = WindowStyle.ToolWindow,
				SizeToContent = SizeToContent.WidthAndHeight,
				DataContext = chooser
			};
			chooser.PropertyChanged += (sender, e) => { if (e.PropertyName.Equals("SelectedCard")) window.Close(); };
			window.ShowDialog();
			return chooser.SelectedCard;
		} 
	}
}