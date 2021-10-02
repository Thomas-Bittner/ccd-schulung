using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

using CsvTable;

namespace Test
{
	public class CsvTableTest
	{
		[Fact]
		public void DisplayCsvTable()
		{
			var csvInput =
				"Name;Strasse;Ort;Alter\n" +
				"Peter Pan;Am Hang 5;12345 Einsam;42\n" +
				"Maria Schmitz;Kölner Straße 45;50123 Köln;43\n" +
				"Paul Meier;Münchener Weg 1;87654 München;65\n" +
				"Bruce Wayne;;Gotham City;36\n";

			var readableOutput = CsvTableCreator.Tabellieren(csvInput);
			readableOutput.Should().Be(
				"Name         |Strasse         |Ort          |Alter|\n" +
				"-------------+----------------+-------------+-----+\n" +
				"Peter Pan    |Am Hang 5       |12345 Einsam |42   |\n" +
				"Maria Schmitz|Kölner Straße 45|50123 Köln   |43   |\n" +
				"Paul Meier   |Münchener Weg 1 |87654 München|65   |\n" +
				"Bruce Wayne  |                |Gotham City  |36   |\n"
			);
		}

		[Fact]
		public void SplitHeader()
		{
			string input = "Name;Strasse;Ort;Alter";
			CsvTableCreator.ExtractHeader(input).Should().Equal("Name", "Strasse", "Ort", "Alter");
		}

		[Fact]
		public void ExtractContent()
		{
			var input = new[]
			{
				"Peter Pan;Am Hang 5;12345 Einsam;42",
				"Maria Schmitz;Kölner Straße 45;50123 Köln;43",
				"Paul Meier;Münchener Weg 1;87654 München;65",
				"Bruce Wayne;;Gotham City;36"
			};

			var content = CsvTableCreator.ExtractContent(input).ToArray();
			content.Length.Should().Be(4);
			content[0].Should().Equal(new List<string>{"Peter Pan", "Am Hang 5", "12345 Einsam", "42"});
			content[1].Should().Equal(new List<string> {"Maria Schmitz", "Kölner Straße 45", "50123 Köln", "43"});
			content[2].Should().Equal(new List<string> {"Paul Meier", "Münchener Weg 1", "87654 München", "65"});
			content[3].Should().Equal(new List<string> {"Bruce Wayne", "", "Gotham City", "36"});
		}

		[Fact]
		public void CalculateColumnWidths()
		{
			var headers = new[] {"Long Title", "A", "B"};
			var content = new List<IEnumerable<string>>
			{
				new [] {"Foo", "LongA", "B"},
				new [] {"Bar", "A", "LongB"}
			};
			var table = new CsvTableCreator(headers, content);
			table.CalculateColumnWidths().Should().Equal(10, 5, 5);
		}

		[Fact]
		public void SerializeAsReadableString()
		{
			var headers = new[] { "Long Title", "A", "B" };
			var content = new List<IEnumerable<string>>
			{
				new[] {"Foo", "LongA", "B"},
				new[] {"Bar", "A", "LongB"}
			};
			var table = new CsvTableCreator(headers, content);
			table.ToString(new int[]{10,5,5}).Should().Be("Long Title|A    |B    |\n----------+-----+-----+\nFoo       |LongA|B    |\nBar       |A    |LongB|\n");
		}
	}
}
