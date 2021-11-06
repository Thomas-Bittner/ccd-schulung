using System.Linq;
using System;
using System.IO;
using System.Text;

namespace HexDump
{
	public static class HexConverter
	{
		public static string ToHexDump(string filePath)
		{
      var result = "";

      using (var input = File.OpenRead(filePath))
      {
        int position = 0;
        var buffer = new byte[16];

        while (position < input.Length)
        {
          var charsRead = input.Read(buffer, 0, buffer.Length);
          if (charsRead > 0)
          {
            result += PositionToHex(position);
            result += ": ";

            position += charsRead;

            for (int i = 0; i < 16; i++)
            {
              result += (i < charsRead) ? CharacterToHex(buffer[i]) + " " : "  ";

              if (i == 7)
              {
                result += "-- ";
              }
            }

            result += "  ";
            result += BufferToString(buffer, charsRead);
            result += "\n";
          }
        }
      }
      return result;
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
