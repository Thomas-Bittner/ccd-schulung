using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HexDump
{
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

		class HexLine
		{
			public int Position { get; set; }
			public byte[] Characters { get; set; }
			public int NumberOfCharacters { get; set; }

			public string ToHexDump()
			{
				var result = PositionToHex(Position);
				result += ": ";
				result += CreateHexString();
				result += "  ";
				result += BufferToString(Characters, NumberOfCharacters);
				result += "\n";
				return result;
			}

			private string CreateHexString()
			{
				var result = "";
				for (var i = 0; i < Characters.Length; i++)
				{
					result += (i < NumberOfCharacters) ? CharacterToHex(Characters[i]) + " " : "  ";
					if (i == 7)
						result += "-- ";
				}
				return result;
			}
		}

		private static string PositionToHex(int position)
			=> string.Format("{0:x4}", position);

		private static string CharacterToHex(byte character)
			=> string.Format("{0:x2}", character);

		private static string BufferToString(byte[] buffer, int charsRead)
		{
			foreach (ref var character in buffer.AsSpan())
				character = ReplaceExoticCharacters(character);
			var bufferContent = Encoding.ASCII.GetString(buffer);
			return bufferContent.Substring(0, charsRead);
		}

		private static byte ReplaceExoticCharacters(byte character)
		{
			if (character < 32 || 250 < character)
				return (byte)'.';
			return character;
		}
	}
}
