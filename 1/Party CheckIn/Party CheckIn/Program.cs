using System;

namespace PartyCheckIn
{
	internal static class Program
	{
		public static void Main()
		{
			var guestDatabase = new GuestDatabase("guests.sqlite");
			guestDatabase.Initialize();

			var checkInManager = new CheckInManager(guestDatabase);

			while (true)
			{
				Console.Write("Name: ");
				var name = Console.ReadLine();
				if (string.IsNullOrEmpty(name))
				{
					Console.WriteLine("Hello, World!");
					continue;
				}

				checkInManager.CheckIn(name);
				var greeting = checkInManager.GetGreeting(name);
				Console.WriteLine(greeting);
			}

			// ReSharper disable once FunctionNeverReturns
		}
	}
}
