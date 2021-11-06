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
            result += string.Format("{0:x4}", position) + ": ";
            position += charsRead;

            for (int i = 0; i < 16; i++)
            {
              if (i < charsRead)
              {
                var hex = string.Format("{0:x2}", buffer[i]);
                result += hex + " ";
              }
              else
              {
                result += "  ";
              }

              if (i == 7)
              {
                result += "-- ";
              }

              if (buffer[i] < 32 || buffer[i] > 250)
              {
                buffer[i] = (byte)'.';
              }
            }

            var bufferContent = Encoding.ASCII.GetString(buffer);
            result += "  " + bufferContent.Substring(0, charsRead) + "\n";
          }
        }
      }
      return result;
    }
	}
}
