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

			if (_connection is null)
				throw new InvalidOperationException("Could not connect to database.");

			var createGuestTable = _connection.CreateCommand();
			createGuestTable.CommandText =
					"CREATE TABLE guests (guestId INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(20) NOT NULL)";
			createGuestTable.ExecuteNonQuery();

			var createCheckInTable = _connection.CreateCommand();
			createCheckInTable.CommandText =
					"CREATE TABLE checkIns (guestId INTEGER UNIQUE, numberOfCheckIns INT DEFAULT 0, FOREIGN KEY (guestId) REFERENCES guests(guestId))";
			createCheckInTable.ExecuteNonQuery();
		}

		private void Connect()
		{
			_connection = new SQLiteConnection($"Data Source={_databasePath}");
			_connection.Open();
		}

		public void CreateGuest(string name)
		{
			if (_connection is null)
				throw new InvalidOperationException("Not connected to database.");

			var createGuest = _connection.CreateCommand();
			createGuest.CommandText = $"INSERT INTO guests (name) VALUES ('{name}')";
			createGuest.ExecuteNonQuery();

			var createCheckIn = _connection.CreateCommand();
			createCheckIn.CommandText = $"INSERT INTO checkIns (guestId) VALUES ({_connection.LastInsertRowId})";
			createCheckIn.ExecuteNonQuery();
		}

		public bool DoesGuestExist(string name)
		{
			if (_connection is null)
				throw new InvalidOperationException("Not connected to database.");

			var selectGuest = _connection.CreateCommand();
			selectGuest.CommandText = $"SELECT * FROM guests WHERE name == '{name}'";
			var reader = selectGuest.ExecuteReader();
			return reader.HasRows;
		}

		public void SetNumberOfCheckIns(string name, int numberOfCheckIns)
		{
			if (_connection is null)
				throw new InvalidOperationException("Not connected to database.");

			var guestId = GetGuestId(name);
			var updateNumberOfCheckIns = _connection.CreateCommand();
			updateNumberOfCheckIns.CommandText =
					$"UPDATE checkIns SET numberOfCheckIns = {numberOfCheckIns} WHERE guestId == '{guestId}'";
			updateNumberOfCheckIns.ExecuteNonQuery();
		}

		public int GetNumberOfCheckIns(string name)
		{
			if (_connection is null)
				throw new InvalidOperationException("Not connected to database.");

			var guestId = GetGuestId(name);
			var selectCheckIns = _connection.CreateCommand();
			selectCheckIns.CommandText = $"SELECT numberOfCheckIns FROM checkIns WHERE guestId == {guestId}";
			var reader = selectCheckIns.ExecuteReader();

			if (!reader.Read() || !reader.HasRows)
				throw new ArgumentException($"No checkin data found for '{name}'.");

			return int.Parse(reader[0].ToString()!);
		}

		private int GetGuestId(string name)
		{
			if (_connection is null)
				throw new InvalidOperationException("Not connected to database.");

			var selectGuestId = _connection.CreateCommand();
			selectGuestId.CommandText = $"SELECT guestId FROM guests WHERE name == '{name}'";
			var reader = selectGuestId.ExecuteReader();

			if (!reader.Read() || !reader.HasRows)
				throw new ArgumentException($"No guest '{name}' found.");

			return int.Parse(reader[0].ToString()!);
		}
	}
}
