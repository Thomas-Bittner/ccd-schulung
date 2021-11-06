using System;
using System.IO;
using System.Text;

using HexDump;

namespace ancientcode
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      if (args.Length != 1)
      {
        Console.Error.WriteLine("Usage: hexdump <dateiname>");
        Environment.Exit(1);
      }

      if (!File.Exists(args[0]))
      {
        Console.Error.WriteLine("No such file: {0}", args[0]);
        Environment.Exit(2);
      }

      var fileAsHexDump = HexConverter.ToHexDump(args[0]);
      Console.WriteLine(fileAsHexDump);
    }
  }
}
