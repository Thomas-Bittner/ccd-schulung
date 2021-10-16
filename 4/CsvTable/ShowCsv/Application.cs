using System;
using System.IO;

using CsvTable;

namespace ShowCsv
{
	interface IOutput
	{
		void Display(string output);
	}

	public class Application
	{
		public char Separator { get; set; } = ';';
		public string FilePath { get; set; }
		public Action<string> Output { get; set; } = Console.WriteLine;

		public Application(string filePath)
		{
			FilePath = filePath;
		}

		public void Run()
		{
			var csvData = File.ReadAllText(FilePath);
			var csvAsTable = CsvTableConverter.Tabellieren(csvData, Separator);
			Output(csvAsTable);
		}
	}
}
