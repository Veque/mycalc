using System.Collections.Generic;
using System.Data.SqlClient;
using MVVM;

namespace MyCalc.Classes {
	public class DBHelper : ADOHelper<string> {
		public string ConnectionString { get; set; }

		public DBHelper(string connString) {
			ConnectionString = connString;
		}

		public override SqlConnection GetConnection(string connectionString) {
			return new SqlConnection(connectionString);
		}

		public void SaveCombinationEnergy(int c1, int c2, int c3, int c4, int c5, int c6, int c7, int energy5) {
			//combination.Cards.Sort((c1, c2) => {
			//	var res = c1.Value.CompareTo(c2.Value);
			//	if (res != 0)
			//		return res;
			//	return c1.Suit.CompareTo(c2.Suit);
			//});
			Execute(ConnectionString, "SaveCombinationEnergy", true, new[]{
				new SqlParameter("@c1",c1), 
				new SqlParameter("c2",c2), 
				new SqlParameter("c3",c3), 
				new SqlParameter("c4",c4), 
				new SqlParameter("c5",c5), 
				new SqlParameter("c6",c6), 
				new SqlParameter("c7",c7), 
				new SqlParameter("e5",energy5), 
				new SqlParameter("e7",CombinationRatings.Conversion5to7[energy5]), 
			});
		}

		public void SaveCombinationEnergyNoFlush(int c1, int c2, int c3, int c4, int c5, int c6, int c7, int energy5) {
			//combination.Cards.Sort((c1, c2) => {
			//	var res = c1.Value.CompareTo(c2.Value);
			//	if (res != 0)
			//		return res;
			//	return c1.Suit.CompareTo(c2.Suit);
			//});
			Execute(ConnectionString, "SaveCombinationEnergyNoFlush", true, new[]{
				new SqlParameter("@c1",c1), 
				new SqlParameter("c2",c2), 
				new SqlParameter("c3",c3), 
				new SqlParameter("c4",c4), 
				new SqlParameter("c5",c5), 
				new SqlParameter("c6",c6), 
				new SqlParameter("c7",c7), 
				new SqlParameter("e5",energy5), 
				new SqlParameter("e7",CombinationRatings.Conversion5to7[energy5]), 
			});
		}

		public Dictionary<int, int> GetConversion() {
			var res = new Dictionary<int, int>();
			GetList(this.ConnectionString, "GetConversion5to7", (r => res[r.GetInt16(1)] = r.GetInt16(0)), true);
			return res;
		}
	}
}