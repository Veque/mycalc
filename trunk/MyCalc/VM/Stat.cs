using System.Diagnostics;

namespace MyCalc.VM{
	[DebuggerDisplay("E={EnergyInt} C={CombinationsInt}")]
	public class Stat {
		public int EnergyInt { get; set; }
		public int CombinationsInt { get; set; }

		public string Energy { get { return EnergyInt.ToString("n0"); } }
		public string Combinations { get { return CombinationsInt.ToString("n0"); } }

		public string Percent { get { return (CombinationsInt / (double)Total).ToString("p2"); } }

		public double Width {
			get { return AverageWidth * (CombinationsInt / (double)Total); }
		}

		public static double AverageWidth { get; set; }
		public static int Total { get; set; }
		static Stat() {
			AverageWidth = 280000;
			Total = 52 * 51 * 50 * 49 * 48 / (5 * 4 * 3 * 2);
		}
	}
}