namespace PartyCheckIn
{
	public interface IGuestRepository
	{
		bool DoesGuestExist(string name);
		void CreateGuest(string name);

		void SetNumberOfCheckIns(string name, int numberOfCheckIns);
		int GetNumberOfCheckIns(string name);
	}
}
