using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;

namespace MyCalc.Classes {
	public class ColorRandomizer {
		static ColorRandomizer() {
			Colors = new List<Brush>(new[]{
				Brushes.Goldenrod,
				Brushes.IndianRed,
				Brushes.Khaki,
				Brushes.LightSteelBlue,
				Brushes.Pink,
				Brushes.YellowGreen,
				Brushes.Turquoise,
				Brushes.Tomato,
				Brushes.Aquamarine,
				Brushes.Coral,
				Brushes.DarkSeaGreen,
			});
			usedColors = new List<Brush>();
		}

		public static List<Brush> Colors { get; private set; }

		private static Random rand = new Random();
		private static List<Brush> usedColors { get; set; }

		public static Brush PickColor() {
			if (usedColors.Count == Colors.Count)
				usedColors.Clear();
			Brush res;
			var list = Colors.Except(usedColors).ToList();
			if (list.Count == 1) {
				res = list[0];
			} else {
				res = list[rand.Next(0, list.Count)];
			}
			usedColors.Add(res);
			return res;
		}
	}
}