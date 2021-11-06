using System.Linq;
using System.Text;

namespace HexDump
{
	static class HexSerialization
	{
		public static string PositionToHex(int position)
			=> string.Format("{0:x4}", position);

		public static string CharacterToHex(byte character)
			=> string.Format("{0:x2}", character);

		public static string BufferToString(byte[] buffer)
		{
			var sanitizedBuffer = buffer.Select(ReplaceExoticCharacters);
			return Encoding.ASCII.GetString(sanitizedBuffer.ToArray());
		}

		private static byte ReplaceExoticCharacters(byte character)
		{
			if (character < 32 || 250 < character)
				return (byte)'.';
			return character;
		}
	}
}
