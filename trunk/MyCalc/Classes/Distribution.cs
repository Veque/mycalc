using System;
using System.Collections.Generic;

namespace MyCalc.Classes {
	public class Distribution {
		public Distribution() {
			Items = new List<DistributionItem>();
			entropy = -1;
			maxPercent = -1;
		}
		public int Total { get; set; }
		public double Average { get; set; }
		public List<DistributionItem> Items { get; set; }

		private double entropy;
		public double Entropy {
			get {
				if (entropy < 0) {
					entropy = 0;
					foreach (var item in Items) {
						var p = (double)item.Count / Total;
						entropy += -1 * Math.Log(p, 2);
					}
				}
				return entropy;
			}
		}

		private double maxPercent;
		public double MaxPercent{get{
			if (maxPercent < 0) {
				maxPercent = 0;
					foreach (var item in Items) {
						var p = (double)item.Count / Total;
						if (p > maxPercent) maxPercent = p;
					}
				}
			return maxPercent;
		}}

		public void Reset(){
			entropy = -1;
		}
	}
}