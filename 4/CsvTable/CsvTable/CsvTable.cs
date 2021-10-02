using System.Linq;

namespace CsvTable
{
	public class CsvTableCreator
	{
		private string[] header;
		private string[,] content;

		public static CsvTableCreator FromCsv(string csv)
		{
			var lines = csv.Split('\n');
			return new CsvTableCreator
			{
				header = ExtractHeader(lines[0]),
				content = ExtractContent(lines.Skip(1).ToArray())
			};
		}

		public static string[] ExtractHeader(string header)
			=> header.Split(';');

		public static string[,] ExtractContent(string[] content)
		{
			var splittedLines = content.Select(line => line.Split(';')).ToArray();
			var result = new string[splittedLines.Length, splittedLines[0].Length];
			for (var row = 0; row < splittedLines.Length; row++)
			{
				for (var column = 0; column < splittedLines[0].Length; column++)
				{
					result[row, column] = splittedLines[row][column];
				}
			}
			return result;
		}
	}
}
