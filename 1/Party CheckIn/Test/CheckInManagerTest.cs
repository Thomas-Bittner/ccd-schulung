using PartyCheckIn;
using Moq;
using Xunit;

namespace Test
{
	public class CheckInManagerTest
	{
		[Theory]
		[InlineData("Roger", 1, "Hello, Roger!")]
		[InlineData("Roger", 2, "Welcome back, Roger!")]
		[InlineData("Roger", 3, "Hello my good friend, Roger!")]
		[InlineData("Roger", 25, "Congrats! You are now a platinum guest!")]
		[InlineData("Roger", 26, "Hello my good friend, Roger!")]
		public void ShouldReturnCorrectGreeting(string name, int numberOfCheckIns, string expectedGreeting)
		{
			var guestRepositoryMock = new Mock<IGuestRepository>();
			guestRepositoryMock.Setup(x => x.GetNumberOfCheckIns(name)).Returns(numberOfCheckIns);

			var manager = new CheckInManager(guestRepositoryMock.Object);
			Assert.Equal(expectedGreeting, manager.GetGreeting(name));
		}
	}
}
