namespace MyCalc.Classes {
	public class Combinatorics {
		public static long C(int from, int n) {
			long res = 1;
			for (int i = (from - n+1); i <= from; i++) {
				res *= i;
			}
			for (int i = 2; i <= n; i++) {
				res /= i;
			}
			return res;
		}
	}
}