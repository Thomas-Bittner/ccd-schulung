using System;

namespace ShowCsv
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 1)
			{
				var application = new Application(args[0]);
				application.Run();
			}
			else if (args.Length == 2)
			{
				var application = new Application(args[1])
				{
					Separator = args[0][0],
				};
				application.Run();
			}
			else
			{
				ShowHelp();
			}
		}

		private static void ShowHelp()
		{
			Console.WriteLine("Usage: showcsv [<Trennzeichen>] <Dateiname>");
		}
	}
}
