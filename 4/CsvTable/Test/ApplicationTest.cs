using System;
using System.IO;

using FluentAssertions;
using NSubstitute;
using Xunit;

using ShowCsv;

namespace Test
{
	public class ApplicationTest
	{
		[Fact]
		public void CheckRunWithPersonenCsv()
		{
			File.WriteAllText("personen.csv",
				"Name;Strasse;Ort;Alter\n" +
				"Peter Pan;Am Hang 5;12345 Einsam;42\n" +
				"Maria Schmitz;Kölner Straße 45;50123 Köln;43\n" +
				"Paul Meier;Münchener Weg 1;87654 München;65\n" +
				"Bruce Wayne;;Gotham City;36");

			var receivedOutput = "";
			var application = new Application("personen.csv")
			{
				Output = output => receivedOutput = output
			};
			application.Run();
			receivedOutput.Should().Be(
				"Name         |Strasse         |Ort          |Alter|\n" +
				"-------------+----------------+-------------+-----+\n" +
				"Peter Pan    |Am Hang 5       |12345 Einsam |42   |\n" +
				"Maria Schmitz|Kölner Straße 45|50123 Köln   |43   |\n" +
				"Paul Meier   |Münchener Weg 1 |87654 München|65   |\n" +
				"Bruce Wayne  |                |Gotham City  |36   |\n"
			);
		}
	}
}
