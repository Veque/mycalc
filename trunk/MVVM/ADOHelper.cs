using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MVVM {
	public abstract class ADOHelper<TConnection> {

		public class DBHelperException : Exception {
			public DBHelperException(string query, Exception exception)
				: base("SCRIPT: " + query + "\n\nEXCEPTION: " + exception.Message, exception) { }
		}

		public int SqlCmdTimeoutSeconds { get; set; }
		protected virtual string[] CommandDelimiters { get { return new string[] { "\nGO\n", "\r\nGO\r\n" }; } }
		protected virtual string InvalidScriptEnd { get { return "\nGO"; } }
		public abstract SqlConnection GetConnection(TConnection conInfo);

		#region privates
		private SqlCommand createCommand(SqlConnection connection, string procName, bool isProcedure) {
			SqlCommand ret = connection.CreateCommand();
			ret.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
			ret.CommandText = procName;

			ret.CommandTimeout = SqlCmdTimeoutSeconds;

			return ret;
		}

		private SqlCommand createCommand(SqlTransaction transaction, string procName, bool isProcedure) {
			SqlCommand ret = transaction.Connection.CreateCommand();
			ret.Transaction = transaction;
			ret.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
			ret.CommandText = procName;

			ret.CommandTimeout = SqlCmdTimeoutSeconds;

			return ret;
		}

		private SqlDataReader getReader(SqlConnection connection, SqlTransaction transaction, string procName, bool isProcedure, params SqlParameter[] parameters) {
			SqlCommand command = null;
			if (transaction == null)
				command = createCommand(connection, procName, isProcedure);
			else
				command = createCommand(transaction, procName, isProcedure);

			command.Parameters.AddRange((SqlParameter[]) parameters.Select(p => new SqlParameter(p.ParameterName, p.Value)).ToArray());

			try {
				return command.ExecuteReader();
			} catch (SqlException e) {
				throw new DBHelperException(procName, e);
			}
		}

		private List<T> getList<T>(SqlConnection connection, SqlTransaction transaction, string procName, Func<SqlDataReader, T> getter, bool isProcedure, params SqlParameter[] parameters) {
			SqlDataReader reader = null;
			if (transaction == null)
				reader = getReader(connection, null, procName, isProcedure, parameters);
			else
				reader = getReader(transaction.Connection, transaction, procName, isProcedure, parameters);

			try {
				var result = new List<T>();

				while (reader.Read()) {
					result.Add(getter(reader));
				}

				return result;
			} finally {
				reader.Close();
			}
		}

		private T getScalar<T>(SqlConnection connection, SqlTransaction transaction, string procName, Func<SqlDataReader, T> getter, bool isProcedure, params SqlParameter[] parameters) {
			SqlDataReader reader = null;
			if (transaction == null)
				reader = getReader(connection, null, procName, isProcedure, parameters);
			else
				reader = getReader(transaction.Connection, transaction, procName, isProcedure, parameters);


			try {
				if (reader.Read()) {
					return getter(reader);
				}

				return default(T);
			} finally {
				reader.Close();
			}
		}

		private SqlDataReader getReader(SqlConnection connection, string procName, params SqlParameter[] parameters) {
			return getReader(connection, transaction: null, procName: procName, isProcedure: false, parameters: parameters);
		}

		private string[] getScripts(string procName, bool isProcedure) {
			var scripts = new string[] { procName };

			if (procName.EndsWith(InvalidScriptEnd)) {
				procName = procName.Substring(0, procName.Length - InvalidScriptEnd.Length);
			}

			if (!isProcedure) {
				scripts = procName.Split(this.CommandDelimiters, StringSplitOptions.RemoveEmptyEntries);
			}
			return scripts;
		}

		private void execute(SqlConnection connection, SqlTransaction transaction, string procName, bool isProcedure, params SqlParameter[] parameters) {
			if (connection.State != ConnectionState.Open)
				connection.Open();
			foreach (var script in getScripts(procName, isProcedure)) {
				SqlCommand command = null;
				if (transaction == null) {
					command = createCommand(connection, script, isProcedure);
				} else {
					command = createCommand(transaction, script, isProcedure);
				}
				command.Parameters.AddRange(parameters.Select(p => new SqlParameter(p.ParameterName, p.Value)).ToArray());

				try {
					command.ExecuteNonQuery();
				} catch (SqlException e) {
					throw new DBHelperException(procName, e);
				}
			}
		}

		#endregion

		#region publics

		#region List

		#region SqlConnection

		public List<T> GetList<T>(SqlConnection connection, string procName, Func<SqlDataReader, T> getter, bool isProcedure, params SqlParameter[] parameters) {
			return getList(connection, null, procName, getter, isProcedure, parameters);
		}

		public List<T> GetList<T>(SqlConnection connection, string procName, bool isProcedure, params SqlParameter[] parameters) {
			return GetList<T>(connection, procName, reader => (T)reader[0], isProcedure, parameters);
		}

		public List<T> GetList<T>(SqlConnection connection, string procName, Func<SqlDataReader, T> getter, params SqlParameter[] parameters) {
			return GetList(connection, procName, getter, false, parameters);
		}

		public List<T> GetList<T>(SqlConnection connection, string procName, params SqlParameter[] parameters) {
			return GetList(connection, procName, reader => (T)reader[0], parameters);
		}

		#endregion

		#region TConnection

		public List<T> GetList<T>(TConnection conInfo, string procName, Func<SqlDataReader, T> getter, bool isProcedure, params SqlParameter[] parameters) {
			using (var con = GetConnection(conInfo)) {
				con.Open();
				return getList(con, null, procName, getter, isProcedure, parameters);
			}
		}

		public List<T> GetList<T>(TConnection conInfo, string procName, bool isProcedure, params SqlParameter[] parameters) {
			return GetList<T>(conInfo, procName, reader => (T)reader[0], isProcedure, parameters);
		}

		public List<T> GetList<T>(TConnection conInfo, string procName, Func<SqlDataReader, T> getter, params SqlParameter[] parameters) {
			return GetList(conInfo, procName, getter, false, parameters);
		}

		public List<T> GetList<T>(TConnection conInfo, string procName, params SqlParameter[] parameters) {
			return GetList(conInfo, procName, reader => (T)reader[0], parameters);
		}

		#endregion

		#region SqlTransaction

		public List<T> GetList<T>(SqlTransaction transaction, string procName, Func<SqlDataReader, T> getter, bool isProcedure, params SqlParameter[] parameters) {
			return getList(transaction.Connection, transaction, procName, getter, isProcedure, parameters);
		}

		public List<T> GetList<T>(SqlTransaction transaction, string procName, bool isProcedure, params SqlParameter[] parameters) {
			return GetList(transaction, procName, reader => (T)reader[0], isProcedure, parameters);
		}

		public List<T> GetList<T>(SqlTransaction transaction, string procName, Func<SqlDataReader, T> getter, params SqlParameter[] parameters) {
			return GetList(transaction, procName, getter, false, parameters);
		}

		public List<T> GetList<T>(SqlTransaction transaction, string procName, params SqlParameter[] parameters) {
			return GetList(transaction, procName, reader => (T)reader[0], parameters);
		}

		#endregion

		#endregion

		#region Scalar

		#region TConnection

		public T GetScalar<T>(TConnection conInfo, string procName, Func<SqlDataReader, T> getter, bool isProcedure, params SqlParameter[] parameters) {
			using (var con = GetConnection(conInfo)) {
				con.Open();
				return getScalar(con, null, procName, getter, isProcedure, parameters);
			}
		}

		public T GetScalar<T>(TConnection conInfo, string procName, bool isProcedure, params SqlParameter[] parameters) {
			return GetScalar(conInfo, procName, reader => (T)reader[0], isProcedure, parameters);
		}

		public T GetScalar<T>(TConnection conInfo, string procName, params SqlParameter[] parameters) {
			return GetScalar(conInfo, procName, reader => (T)reader[0], parameters);
		}

		public T GetScalar<T>(TConnection conInfo, string procName, Func<SqlDataReader, T> getter, params SqlParameter[] parameters) {
			return GetScalar(conInfo, procName, getter, false, parameters);
		}

		#endregion

		#region SqlTransaction

		public T GetScalar<T>(SqlTransaction transaction, string procName, Func<SqlDataReader, T> getter, bool isProcedure, params SqlParameter[] parameters) {
			return getScalar(transaction.Connection, transaction, procName, getter, isProcedure, parameters);
		}

		public T GetScalar<T>(SqlTransaction transaction, string procName, Func<SqlDataReader, T> getter, params SqlParameter[] parameters) {
			return GetScalar(transaction, procName, getter, false, parameters);
		}

		public T GetScalar<T>(SqlTransaction transaction, string procName, bool isProcedure, params SqlParameter[] parameters) {
			return GetScalar(transaction, procName, reader => (T)reader[0], isProcedure, parameters);
		}

		public T GetScalar<T>(SqlTransaction transaction, string procName, params SqlParameter[] parameters) {
			return GetScalar(transaction, procName, reader => (T)reader[0], parameters);
		}

		#endregion

		#region SqlConnection

		public T GetScalar<T>(SqlConnection connection, string procName, Func<SqlDataReader, T> getter, bool isProcedure, params SqlParameter[] parameters) {
			return getScalar(connection, null, procName, getter, isProcedure, parameters);
		}

		public T GetScalar<T>(SqlConnection connection, string procName, Func<SqlDataReader, T> getter, params SqlParameter[] parameters) {
			return GetScalar(connection, procName, getter, false, parameters);
		}

		#endregion

		#endregion

		#region Execute

		#region TConnection

		public void Execute(TConnection conInfo, string procName, bool isProcedure, params SqlParameter[] parameters) {
			using (var con = GetConnection(conInfo)) {
				con.Open();
				Execute(con, procName, isProcedure, parameters);
			}
		}

		public void Execute(TConnection conInfo, string procName, params SqlParameter[] parameters) {
			Execute(conInfo, procName, false, parameters);
		}

		#endregion

		#region SqlConnection

		public void Execute(SqlConnection connection, string procName, bool isProcedure, params SqlParameter[] parameters) {
			execute(connection, null, procName, isProcedure, parameters);
		}

		public void Execute(SqlConnection connection, string procName, params SqlParameter[] parameters) {
			Execute(connection, procName, false, parameters);
		}

		#endregion

		#region SqlTransaction

		public void Execute(SqlTransaction transaction, string procName, bool isProcedure, params SqlParameter[] parameters) {
			execute(transaction.Connection, transaction, procName, isProcedure, parameters);
		}

		public void Execute(SqlTransaction transaction, string procName, params SqlParameter[] parameters) {
			Execute(transaction, procName, false, parameters);
		}

		#endregion

		#endregion

		#region transactions

		public SqlTransaction BeginADOTranscation(TConnection conInfo) {

			var connection = GetConnection(conInfo);
			connection.Open();
			try {
				return connection.BeginTransaction();
			} catch (Exception e) {
				connection.Close();
				throw e;
			}
		}

		public SqlConnection BeginSqlTransaction(TConnection conInfo) {
			var connection = GetConnection(conInfo);
			connection.Open();
			try {
				Execute(connection, "BEGIN TRANSACTION", false);
				return connection;
			} catch (Exception e) {
				connection.Close();
				throw e;
			}
		}

		public void CommitSqlTransaction(SqlConnection connection) {
			try {
				Execute(connection, "COMMIT TRANSACTION", false);
			} finally {
				connection.Close();
			}
		}

		public void RollbackSqlTransaction(SqlConnection connection) {
			try {
			Execute(connection, "ROLLBACK TRANSACTION", false);
			} finally{
			connection.Close();
			}
		}

		#endregion

		#endregion
	}
}
