using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Mandelbrot.Classes;
using MVVM;

namespace Mandelbrot.VM {
	public class MandelbrotVM : HeaderOverlayVM {

		private RectangleD DefaultField { get; set; }

		#region properties

		private SolidColorBrush _selectedAllColors;
		public SolidColorBrush SelectedAllColors {
			get { return _selectedAllColors; }
			set {
				_selectedAllColors = value;
				OnPropertiesChanged("SelectedAllColors");
			}
		}

		private SolidColorBrush _selectedColor;
		public SolidColorBrush SelectedColor {
			get { return _selectedColor; }
			set {
				_selectedColor = value;
				OnPropertiesChanged("SelectedColor");
			}
		}

		private ObservableCollection<SolidColorBrush> _colors;
		public ObservableCollection<SolidColorBrush> Colors {
			get { return _colors; }
			set {
				_colors = value;
				OnPropertiesChanged("Colors");
			}
		}

		private ObservableCollection<MandelbrotImage> _images;
		public ObservableCollection<MandelbrotImage> Images {
			get { return _images; }
			set {
				_images = value;
				OnPropertiesChanged("Images");
			}
		}

		private double _dpiX;
		public double DpiX {
			get { return _dpiX; }
			set {
				_dpiX = value;
				OnPropertiesChanged("DpiX");
			}
		}

		private double _dpiY;
		public double DpiY {
			get { return _dpiY; }
			set {
				_dpiY = value;
				OnPropertiesChanged("DpiY");
			}
		}

		public WriteableBitmap Image {
			get { return SelectedImage == null ? null : SelectedImage.Image; }
		}

		private MandelbrotImage _selectedImage;
		public MandelbrotImage SelectedImage {
			get { return _selectedImage; }
			set {
				_selectedImage = value;
				OnPropertiesChanged("SelectedImage", "Image");
			}
		}

		private double _rectLeft;
		public double RectLeft {
			get { return _rectLeft; }
			set {
				_rectLeft = value;
				OnPropertiesChanged("RectLeft");
			}
		}

		private double _rectTop;
		public double RectTop {
			get { return _rectTop; }
			set {
				_rectTop = value;
				OnPropertiesChanged("RectTop");
			}
		}

		private double _rectWidth;
		public double RectWidth {
			get { return _rectWidth; }
			set {
				_rectWidth = value;
				OnPropertiesChanged("RectWidth");
			}
		}

		private double _rectHeight;
		public double RectHeight {
			get { return _rectHeight; }
			set {
				_rectHeight = value;
				OnPropertiesChanged("RectHeight");
			}
		}

		private int _pixelWidth;
		public int PixelWidth {
			get { return _pixelWidth; }
			set {
				_pixelWidth = value;
				OnPropertiesChanged("PixelWidth");
			}
		}

		private int _pixelHeight;
		public int PixelHeight {
			get { return _pixelHeight; }
			set {
				_pixelHeight = value;
				OnPropertiesChanged("PixelHeight");
			}
		}

		private int _iterationsLimit;
		public int IterationsLimit {
			get { return _iterationsLimit; }
			set {
				_iterationsLimit = value;
				OnPropertiesChanged("IterationsLimit");
			}
		}

		private double _limit;
		public double Limit {
			get { return _limit; }
			set {
				_limit = value;
				OnPropertiesChanged("Limit");
			}
		}

		#endregion

		#region commands
		public Command DrawImageCommand {
			get {
				return new Command(s => {
					var rect = new RectangleD {
						Left = SelectedImage.Field.Left + (RectLeft / PixelWidth) * SelectedImage.Field.Width,
						Top = SelectedImage.Field.Top + (RectTop / PixelHeight) * SelectedImage.Field.Height,
						Width = (RectWidth / PixelWidth) * SelectedImage.Field.Width,
						Height = (RectHeight / PixelHeight) * SelectedImage.Field.Height
					};
					drawNewMandelbrot(rect);
				});
			}
		}

		public Command ResetCommand {
			get {
				return new Command(s => {
					reset();
				});
			}
		}

		public Command SaveCommand {
			get {
				return new Command(s => {
					var dlg = new Microsoft.Win32.SaveFileDialog();
					dlg.FileName = "Mandelbrot";
					dlg.DefaultExt = ".png";
					dlg.Filter = "bitmap (.png)|*.png";

					var result = dlg.ShowDialog();
					if (result.Value) {
						if (dlg.FileName != string.Empty) {
							using (var stream = new FileStream(dlg.FileName, FileMode.Create)) {
								var encoder = new PngBitmapEncoder();
								encoder.Frames.Add(BitmapFrame.Create(Image));
								encoder.Save(stream);
								stream.Close();
							}
						}
					}
				});
			}
		}

		public Command RedrawCommand {
			get {
				return new Command(_ => new Thread(() => {
					ShowOverlay = true;
					try {
						SelectedImage.Image = drawImage(SelectedImage.Field);
						OnPropertiesChanged("SelectedImage", "Images", "Image");
					}
					finally {
						ShowOverlay = false;
					}
				}).Start());
			}
		}

		public Command AddColorCommand {
			get {
				return new Command(s => Colors.Add(SelectedAllColors), s => SelectedAllColors != null);
			}
		}

		public Command RemoveColorCommand {
			get {
				return new Command(s => {
					var index = Colors.IndexOf(SelectedColor);
					Colors.Remove(SelectedColor);
					if (index < Colors.Count)
						SelectedColor = Colors[index];
					else if (index > 0)
						SelectedColor = Colors[index - 1];
					else
						SelectedColor = null;
				}, s => SelectedColor != null);
			}
		}

		public Command MoveLeftColorCommand {
			get {
				return new Command(s => {
					var index = Colors.IndexOf(SelectedColor);
					if (index == 0) return;
					Colors.Move(index, index - 1);
				}, s => SelectedColor != null);
			}
		}

		public Command MoveRightColorCommand {
			get {
				return new Command(s => {
					var index = Colors.IndexOf(SelectedColor);
					if (index == Colors.Count - 1) return;
					Colors.Move(index, index + 1);
				}, s => SelectedColor != null);
			}
		}

		#endregion

		public MandelbrotVM(Dispatcher dispatcher)
			: base(dispatcher) {
			_images = new ObservableCollection<MandelbrotImage>();
			_dpiX = 96;
			_dpiY = 96;

			_pixelWidth = 1200;
			_pixelHeight = 675;


			_rectLeft = 0;
			_rectTop = 0;
			_rectHeight = 300;
			_rectWidth = _rectHeight * _pixelWidth / PixelHeight;
			_limit = 2;
			_iterationsLimit = 2000;

			var initialScale = 2.4;
			var rect = new RectangleD { Left = -0.4 - initialScale * (_pixelWidth / (double)_pixelHeight) / 2, Top = -initialScale / 2, Height = initialScale, Width = initialScale * (_pixelWidth / (double)_pixelHeight) };
			DefaultField = rect;

			reset();
		}

		private void reset() {
			initColors();
			Images.Clear();
			drawNewMandelbrot(DefaultField);
		}

		private void drawNewMandelbrot(RectangleD field) {
			new Thread(() => {
				ShowOverlay = true;
				try {
					var mandelbrot = new MandelbrotImage { Field = field };
					mandelbrot.Image = drawImage(field);
					UIDispatcher.Invoke(() => Images.Add(mandelbrot));
					SelectedImage = mandelbrot;
				}
				finally {
					ShowOverlay = false;
				}
			}).Start();
		}

		private void initColors() {
			var colors = new List<Color>();
			colors.Add(System.Windows.Media.Colors.WhiteSmoke);
			colors.Add(System.Windows.Media.Colors.Firebrick);
			colors.Add(System.Windows.Media.Colors.Firebrick);
			colors.Add(System.Windows.Media.Colors.Firebrick);
			colors.Add(System.Windows.Media.Colors.Maroon);
			colors.Add(System.Windows.Media.Colors.Maroon);
			colors.Add(System.Windows.Media.Colors.Maroon);
			colors.Add(System.Windows.Media.Colors.Maroon);
			colors.Add(System.Windows.Media.Colors.Maroon);
			colors.Add(System.Windows.Media.Colors.Maroon);
			colors.Add(System.Windows.Media.Colors.Black);
			colors.Add(System.Windows.Media.Colors.Black);
			Colors = new ObservableCollection<SolidColorBrush>(colors.Select(c => new SolidColorBrush(c)));
		}

		private WriteableBitmap drawImage(RectangleD rect) {
			var img = new WriteableBitmap(PixelWidth, PixelHeight, DpiX, DpiY, PixelFormats.Bgr32, null);
			var bytesPerPixel = img.Format.BitsPerPixel / 8;
			var stride = PixelWidth * bytesPerPixel;
			//var rect = new RectangleD { Left = RectLeft, Top = RectTop, Width = RectRight, Height = RectBottom };
			byte[] pixels = new byte[PixelHeight * stride];

			var map = MandelbrotCalculator.Calculate(rect, PixelWidth, PixelHeight, Limit, IterationsLimit);

			getPixels(stride, img, map, pixels);

			img.WritePixels(new Int32Rect(0, 0, _pixelWidth, _pixelHeight), pixels, stride, 0);
			img.Freeze();
			//UIDispatcher.Invoke(new Action(() => Image = img));
			return img;
		}

		private void getPixels(int stride, WriteableBitmap img, int[,] map, byte[] pixels) {
			var rInterpolator = new Interpolation();
			var gInterpolator = new Interpolation();
			var bInterpolator = new Interpolation();

			var step = IterationsLimit / (double)(Colors.Count - 1);
			var cnt = 0;
			foreach (var brush in Colors) {
				var color = default(Color);
				UIDispatcher.Invoke(new Action(() => color = brush.Color));

				rInterpolator.Hooks.Add(new PointD { X = step * cnt, Y = color.R });
				gInterpolator.Hooks.Add(new PointD { X = step * cnt, Y = color.G });
				bInterpolator.Hooks.Add(new PointD { X = step * cnt, Y = color.B });
				cnt++;
			}

			for (int x = 0; x < _pixelWidth; x++) {
				for (int y = 0; y < _pixelHeight; y++) {
					var coord = y * stride + x * (img.Format.BitsPerPixel / 8);
					var value = map[x, y];
					var color = (byte)((1 - value / (float)IterationsLimit) * 0xff);
					//byte color = map[x, y] == IterationsLimit ? (byte)0x00 : (byte)0xff;
					pixels[coord] = (byte)(ushort)bInterpolator.F(value);
					pixels[coord + 1] = (byte)(ushort)gInterpolator.F(value);
					pixels[coord + 2] = (byte)(ushort)rInterpolator.F(value);
					//pixels[coord + 3] = 0xff;
				}
			}
		}
	}
}