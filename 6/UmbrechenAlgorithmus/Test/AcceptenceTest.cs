using FluentAssertions;
using Xunit;

using UmbrechenAlgorithmus;

namespace Test
{
	public class AcceptenceTest
	{
		[Fact]
		public void AcceptenceTest1()
		{
			var text = "Es blaut die Nacht,\ndie Sternlein blinken,\nSchneeflöcklein leis herniedersinken.";
			var maxZeilenlänge = 9;
			var umgebrochenerText = UmbrechenClass.Umbrechen(text, maxZeilenlänge);
			umgebrochenerText.Should().Be("Es blaut\ndie\nNacht,\ndie\nSternlein\nblinken,\nSchneeflö\ncklein\nleis\nhernieder\nsinken.");
		}

		[Fact]
		public void AcceptenceTest2()
		{
			var text = "Es blaut die Nacht,\ndie Sternlein blinken,\nSchneeflöcklein leis herniedersinken.";
			var maxZeilenlänge = 14;
			var umgebrochenerText = UmbrechenClass.Umbrechen(text, maxZeilenlänge);
			umgebrochenerText.Should().Be("Es blaut die\nNacht, die\nSternlein\nblinken,\nSchneeflöcklei\nn leis\nherniedersinke\nn.");
		}
	}
}
