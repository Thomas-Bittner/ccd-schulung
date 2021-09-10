namespace PartyCheckIn
{
	public class CheckInManager
	{
		private IGuestRepository guestRepository;

		public CheckInManager(IGuestRepository guestRepository)
		{
			this.guestRepository = guestRepository;
		}

		public void CheckIn(string name)
		{
			if (!guestRepository.DoesGuestExist(name))
				guestRepository.CreateGuest(name);

			var currentNumberOfCheckIns = guestRepository.GetNumberOfCheckIns(name);
			guestRepository.SetNumberOfCheckIns(name, currentNumberOfCheckIns + 1);
		}

		public string GetGreeting(string name)
		{
			var numberOfCheckIns = guestRepository.GetNumberOfCheckIns(name);
			return CreateGreeting(name, numberOfCheckIns);
		}

		private static string CreateGreeting(string name, int numberOfCheckIns)
		{
			if (numberOfCheckIns == 25)
				return "Congrats! You are now a platinum guest!";
			if (numberOfCheckIns >= 3)
				return $"Hello my good friend, {name}!";
			if (numberOfCheckIns == 2)
				return $"Welcome back, {name}!";
			return $"Hello, {name}!";
		}
	}
}
