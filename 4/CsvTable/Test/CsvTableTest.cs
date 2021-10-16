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
			const string csvInput = "Name;Strasse;Ort;Alter\n" +
			                        "Peter Pan;Am Hang 5;12345 Einsam;42\n" +
			                        "Maria Schmitz;Kölner Straße 45;50123 Köln;43\n" +
			                        "Paul Meier;Münchener Weg 1;87654 München;65\n" +
			                        "Bruce Wayne;;Gotham City;36\n";

			var readableOutput = CsvTableConverter.Tabellieren(csvInput);
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
		public void CreateCsvConverterFromCsv()
		{
			const string csvInput = "Name;Strasse;Ort;Alter\n" +
			                        "Peter Pan;Am Hang 5;12345 Einsam;42\n" +
			                        "Maria Schmitz;Kölner Straße 45;50123 Köln;43\n" +
			                        "Paul Meier;Münchener Weg 1;87654 München;65\n" +
			                        "Bruce Wayne;;Gotham City;36\n";

			var converter = CsvTableConverter.FromCsv(csvInput);
			converter.Header.Should().Equal("Name", "Strasse", "Ort", "Alter");

			var content = converter.Content.ToArray();
			content.Length.Should().Be(4);
			content[0].Should().Equal(new List<string> { "Peter Pan", "Am Hang 5", "12345 Einsam", "42" });
			content[1].Should().Equal(new List<string> { "Maria Schmitz", "Kölner Straße 45", "50123 Köln", "43" });
			content[2].Should().Equal(new List<string> { "Paul Meier", "Münchener Weg 1", "87654 München", "65" });
			content[3].Should().Equal(new List<string> { "Bruce Wayne", "", "Gotham City", "36" });
		}

		[Fact]
		public void SerializeAsReadableString()
		{
			var table = new CsvTableConverter
			{
				Header = new[] { "Long Title", "A", "B" },
				Content = new List<IEnumerable<string>>
				{
					new [] {"Foo", "LongA", "B"},
					new [] {"Bar", "A", "LongB"}
				}
			};

			table.ToString().Should().Be(
				"Long Title|A    |B    |\n" +
				"----------+-----+-----+\n" +
				"Foo       |LongA|B    |\n" +
				"Bar       |A    |LongB|\n");
		}
	}
}
