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
		[InlineData("Roger", 4, "Hello my good friend, Roger!")]
		[InlineData("Roger", 24, "Hello my good friend, Roger!")]
		[InlineData("Roger", 25, "Hello my good friend, Roger!\nCongrats! You are now a platinum guest!")]
		[InlineData("Roger", 26, "Hello my good friend, Roger!")]
		public void GetGreeting_ShouldReturnCorrectGreeting(string name, int numberOfCheckIns, string expectedGreeting)
		{
			var guestRepositoryMock = new Mock<IGuestRepository>();
			guestRepositoryMock.Setup(x => x.GetNumberOfCheckIns(name)).Returns(numberOfCheckIns);

			var manager = new CheckInManager(guestRepositoryMock.Object);
			Assert.Equal(expectedGreeting, manager.GetGreeting(name));
		}

		[Fact]
		public void CheckIn_WhenGuestDoesNotExist_ShouldCreateGuest()
		{
			const string name = "Roger";

			var guestRepositoryMock = new Mock<IGuestRepository>();
			guestRepositoryMock.Setup(x => x.DoesGuestExist(name)).Returns(false);

			var manager = new CheckInManager(guestRepositoryMock.Object);
			manager.CheckIn(name);
			guestRepositoryMock.Verify(x => x.CreateGuest(name), Times.Once);
		}

		[Fact]
		public void CheckIn_ShouldIncreaseNumberOfCheckIns()
		{
			const string name = "Roger";

			var guestRepositoryMock = new Mock<IGuestRepository>();
			guestRepositoryMock.Setup(x => x.GetNumberOfCheckIns(name)).Returns(1);

			var manager = new CheckInManager(guestRepositoryMock.Object);
			manager.CheckIn(name);
			guestRepositoryMock.Verify(x => x.SetNumberOfCheckIns(name, 2));
		}
	}
}
