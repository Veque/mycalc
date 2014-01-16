using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using MyCalc.Properties;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace MyCalc.VM {
	public class CardResources {
		public static List<string> Values { get; private set; }
		public static List<BitmapSource> Suits { get; private set; }
		public static List<Brush> SuitColors { get; private set; }

		public static void Init() {
			Values = new List<string>(new[]{
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"10",
				"J",
				"Q",
				"K",
				"A"
			});

			Suits = new List<BitmapSource>(new[]{
				Bitmap2BitmapImage(Resources.clubs),
				Bitmap2BitmapImage(Resources.spades),
				Bitmap2BitmapImage(Resources.diamonds),
				Bitmap2BitmapImage(Resources.hearts)
			});

			SuitColors = new List<Brush>(new[]{
				Brushes.ForestGreen,
				Brushes.Black,
				Brushes.RoyalBlue,
				Brushes.Red
			});
		}

		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		private static BitmapSource Bitmap2BitmapImage(Bitmap bitmap) {
			IntPtr hBitmap = bitmap.GetHbitmap();
			BitmapSource retval = null;

			try {
				retval = Imaging.CreateBitmapSourceFromHBitmap(
							 hBitmap,
							 IntPtr.Zero,
							 Int32Rect.Empty,
							 BitmapSizeOptions.FromEmptyOptions());
			} finally {
				DeleteObject(hBitmap);
			}

			return retval;
		}

	}
}