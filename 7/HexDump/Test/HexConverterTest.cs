using FluentAssertions;
using System.IO;
using Xunit;

using HexDump;

namespace Test
{
	public class HexConverterTest
	{
		[Fact]
		public void CharacterizationTest()
		{
			File.WriteAllText("testFile.txt", "This is a test file.\n We use it to test our HexConverter.");
			var fileAsHexDump = HexConverter.ToHexDump("testFile.txt");
			fileAsHexDump.Should().Be(
				  "0000: 54 68 69 73 20 69 73 20 -- 61 20 74 65 73 74 20 66   This is a test f\n"
				+ "0010: 69 6c 65 2e 0a 20 57 65 -- 20 75 73 65 20 69 74 20   ile.. We use it \n"
				+ "0020: 74 6f 20 74 65 73 74 20 -- 6f 75 72 20 48 65 78 43   to test our HexC\n"
				+ "0030: 6f 6e 76 65 72 74 65 72 -- 2e                 onverter.\n");
		}
	}
}
