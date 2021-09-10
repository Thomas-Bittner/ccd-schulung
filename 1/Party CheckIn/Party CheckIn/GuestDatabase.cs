using System;
using System.Data.SQLite;
using System.IO;

namespace PartyCheckIn
{
	public class GuestDatabase : IGuestRepository
	{
		private readonly string _databasePath;
		private SQLiteConnection? _connection;

		public GuestDatabase(string databasePath)
		{
			this._databasePath = databasePath;
		}

		public void Initialize()
		{
			if (!File.Exists(_databasePath))
				CreateDatabase();
			else
				Connect();
		}

		public void Shutdown()
		{
			_connection?.Close();
		}

		private void CreateDatabase()
		{
			SQLiteConnection.CreateFile(_databasePath);
			Connect();
			ExecuteNonQuery("CREATE TABLE guests (guestId INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(20) NOT NULL)");
			ExecuteNonQuery("CREATE TABLE checkIns (guestId INTEGER UNIQUE, numberOfCheckIns INT DEFAULT 0, FOREIGN KEY (guestId) REFERENCES guests(guestId))");
		}

		private void Connect()
		{
			_connection = new SQLiteConnection($"Data Source={_databasePath}");
			_connection.Open();
		}

		public void CreateGuest(string name)
		{
			ExecuteNonQuery($"INSERT INTO guests (name) VALUES ('{name}')");
			var guestId = GetGuestId(name);
			ExecuteNonQuery($"INSERT INTO checkIns (guestId) VALUES ({guestId})");
		}

		public bool DoesGuestExist(string name)
		{
			return DoesQueryHaveRows($"SELECT * FROM guests WHERE name == '{name}'");
		}

		public void SetNumberOfCheckIns(string name, int numberOfCheckIns)
		{
			var guestId = GetGuestId(name);
			ExecuteNonQuery($"UPDATE checkIns SET numberOfCheckIns = {numberOfCheckIns} WHERE guestId == '{guestId}'");
		}

		public int GetNumberOfCheckIns(string name)
		{
			var guestId = GetGuestId(name);
			return ReadIntegerValueFromDatabase($"SELECT numberOfCheckIns FROM checkIns WHERE guestId == {guestId}");
		}

		private int GetGuestId(string name)
		{
			return ReadIntegerValueFromDatabase($"SELECT guestId FROM guests WHERE name == '{name}'");
		}

		private void ExecuteNonQuery(string nonQuery)
		{
			VerifyConnectionIsValid();
			var nonQueryCommand = _connection!.CreateCommand();
			nonQueryCommand.CommandText = nonQuery;
			nonQueryCommand.ExecuteNonQuery();
		}

		private bool DoesQueryHaveRows(string query)
		{
			VerifyConnectionIsValid();
			var queryCommand = _connection!.CreateCommand();
			queryCommand.CommandText = query;
			var reader = queryCommand.ExecuteReader();
			return reader.HasRows;
		}

		private int ReadIntegerValueFromDatabase(string query)
		{
			VerifyConnectionIsValid();

			var queryCommand = _connection!.CreateCommand();
			queryCommand.CommandText = query;
			var reader = queryCommand.ExecuteReader();

			if (!reader.Read() || !reader.HasRows)
				throw new ArgumentException($"No result for query '{query}' found.");

			return int.Parse(reader[0].ToString() ?? throw new InvalidOperationException("Error reading from database."));
		}

		private void VerifyConnectionIsValid()
		{
			if (_connection is null)
				throw new InvalidOperationException("Not connected to database.");
		}
	}
}
