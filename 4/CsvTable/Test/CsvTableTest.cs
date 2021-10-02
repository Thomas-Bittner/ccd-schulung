using FluentAssertions;
using Xunit;

using CsvTable;

namespace Test
{
	public class CsvTableTest
	{
		[Fact]
		public void SplitHeader()
		{
			string input = "Name;Strasse;Ort;Alter";
			CsvTableCreator.ExtractHeader(input).Should().Equal("Name", "Strasse", "Ort", "Alter");
		}

		[Fact]
		public void ExtractContent()
		{
			string[] input = new string[]
			{
				"Peter Pan;Am Hang 5;12345 Einsam;42",
				"Maria Schmitz;Kölner Straße 45;50123 Köln;43",
				"Paul Meier;Münchener Weg 1;87654 München;65",
				"Bruce Wayne;;Gotham City;36"
			};
			
			Assert.Equal(new string[4,4] {
				{"Peter Pan", "Am Hang 5", "12345 Einsam", "42"},
				{"Maria Schmitz", "Kölner Straße 45", "50123 Köln", "43"},
				{"Paul Meier", "Münchener Weg 1", "87654 München", "65"},
				{"Bruce Wayne", "", "Gotham City", "36"}
			}, CsvTableCreator.ExtractContent(input));
		}
	}
}
