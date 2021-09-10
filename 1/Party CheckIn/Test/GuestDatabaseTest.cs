using System;
using System.IO;
using Xunit;

using PartyCheckIn;

namespace Test
{
	public class GuestDatabaseTest
	{
		private string _databasePath;
		private GuestDatabase _database;

		[Fact]
		public void ShouldCreateDatabase()
		{
			_databasePath = $"{Guid.NewGuid()}.sqlite";
			_database = new GuestDatabase(_databasePath);
			_database.Initialize();
			Assert.True(File.Exists(_databasePath));
			CleanUp();
		}

		[Fact]
		public void ShouldCreateGuest()
		{
			Setup();
			_database.CreateGuest("Roger");
			Assert.True(_database.DoesGuestExist("Roger"));
			CleanUp();
		}

		[Fact]
		public void ShouldSetCheckIn()
		{
			Setup();
			_database.CreateGuest("Roger");
			_database.SetNumberOfCheckIns("Roger", 42);
			Assert.Equal(42, _database.GetNumberOfCheckIns("Roger"));
			CleanUp();
		}

		private void Setup()
		{
			_databasePath = $"{Guid.NewGuid()}.sqlite";
			_database = new GuestDatabase(_databasePath);
			_database.Initialize();
		}

		private void CleanUp()
		{
			_database.Shutdown();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			File.Delete(_databasePath);
		}
	}
}
