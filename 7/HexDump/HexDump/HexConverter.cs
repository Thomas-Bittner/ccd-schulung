using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace HexDump
{
	/*!
	 * \brief The HexConverter creates a printable hex representation of a file.
	 * 
	 * It loads the file in batches of 16 bytes, and serializes such a batch in one line.
	 */
	public static class HexConverter
	{
		public static string ToHexDump(string filePath)
		{
			var hexLines = CreateHexLinesFromFile(filePath);
			var hexDumpLines = hexLines.Select(line => line.ToHexDump());
			return string.Join("", hexDumpLines);
		}

		private static IEnumerable<HexLine> CreateHexLinesFromFile(string filePath)
		{
			using var inputStream = File.OpenRead(filePath);
			var bufferSize = 16;
			for (var position = 0; position < inputStream.Length; position += bufferSize)
			{
				var buffer = new byte[bufferSize];
				var charactersRead = inputStream.Read(buffer, 0, buffer.Length);
				yield return new HexLine
				{
					Position = position,
					Characters = buffer,
					NumberOfCharacters = charactersRead
				};
			}
		}
	}
}
