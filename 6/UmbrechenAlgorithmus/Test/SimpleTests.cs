using FluentAssertions;
using Xunit;

using UmbrechenAlgorithmus;

namespace Test
{
	public class SimpleTests
	{
		[Fact]
		public void ErstelleWortListeTest()
		{
			var text = "foo\nbar bla";
			var wortListe = UmbrechenClass.ErstelleWortListe(text);
			wortListe.Should().BeEquivalentTo(new string[] { "foo", "bar", "bla" });
		}

		[Fact]
		public void FügeZeilenZusammen()
		{
			var zeilen = new string[] { "foo bar", "bla" };
			var text = UmbrechenClass.FügeZeilenZusammen(zeilen);
			text.Should().Be("foo bar\nbla");
		}
	}
}
