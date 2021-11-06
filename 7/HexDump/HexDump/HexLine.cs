namespace HexDump
{
	/*!
	 * \brief The HexLine represents a number of bytes to be serialized.
	 * 
	 * It stores the position of the bytes in the file, a character buffer and the actual number
	 * of characters stored in that buffer (the last line may not fill the whole buffer).
	 * 
	 * The result of the serialization of a line performed by ToHexDump() may look like this:
	 * "0000: 54 68 69 73 20 69 73 20 -- 61 20 74 65 73 74 20 66   This is a test f\n"
	 */
	class HexLine
	{
		private readonly int _position;
		private readonly byte[] _characters;
		private readonly int _numberOfCharacters;

		public HexLine(int position, byte[] characters, int numberOfCharacters)
		{
			_position = position;
			_characters = characters;
			_numberOfCharacters = numberOfCharacters;
		}

		public string ToHexDump()
		{
			var result = HexSerialization.PositionToHex(_position);
			result += ": ";
			result += CreateHexString();
			result += "  ";
			result += HexSerialization.BufferToString(_characters[.._numberOfCharacters]);
			result += "\n";
			return result;
		}

		private string CreateHexString()
		{
			var result = "";
			for (var i = 0; i < _characters.Length; i++)
			{
				if (i < _numberOfCharacters)
				{
					result += HexSerialization.CharacterToHex(_characters[i]);
					result += " ";
				}
				else
				{
					result += "  ";
				}

				if (i == 7)
					result += "-- ";
			}
			return result;
		}
	}
}
