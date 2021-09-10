namespace PartyCheckIn
{
	public class CheckInManager
	{
		private readonly IGuestRepository _guestRepository;

		public CheckInManager(IGuestRepository guestRepository)
		{
			this._guestRepository = guestRepository;
		}

		public void CheckIn(string name)
		{
			if (!_guestRepository.DoesGuestExist(name))
				_guestRepository.CreateGuest(name);

			var currentNumberOfCheckIns = _guestRepository.GetNumberOfCheckIns(name);
			_guestRepository.SetNumberOfCheckIns(name, currentNumberOfCheckIns + 1);
		}

		public string GetGreeting(string name)
		{
			var numberOfCheckIns = _guestRepository.GetNumberOfCheckIns(name);
			return CreateGreeting(name, numberOfCheckIns);
		}

		private static string CreateGreeting(string name, int numberOfCheckIns)
		{
			return numberOfCheckIns switch
			{
				25 => $"Hello my good friend, {name}!\nCongrats! You are now a platinum guest!",
				>= 3 => $"Hello my good friend, {name}!",
				2 => $"Welcome back, {name}!",
				_ => $"Hello, {name}!"
			};
		}
	}
}
