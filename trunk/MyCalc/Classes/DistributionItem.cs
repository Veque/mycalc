namespace MyCalc.Classes{
	public class DistributionItem{
		public int LowValue { get; set; }
		public int HighValue { get; set; }
		public int Count { get; set; }
		public string Title { get; set; }

		public Distribution Distribution { get; set; }
	}
}