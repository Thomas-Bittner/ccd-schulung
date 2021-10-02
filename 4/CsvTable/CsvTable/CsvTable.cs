using System;
using System.Linq;

namespace CsvTable
{
	public class CsvTableCreator
	{
		private string[] _header;
		private string[,] _content;

		public static CsvTableCreator FromCsv(string csv)
		{
			var lines = csv.Split('\n');
			var header = ExtractHeader(lines[0]);
			var content = ExtractContent(lines.Skip(1).ToArray());
			return new CsvTableCreator(header, content);
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

		public CsvTableCreator(string[] header, string[,] content)
		{
			this._header = header;
			this._content = content;
		}

		public int[] CalculateColumnWidths()
		{
			var result = _header.Select(title => title.Length).ToArray();
			for (var row = 0; row < _content.GetLength(0); row++)
			{
				for (var column = 0; column < _content.GetLength(1); column++)
				{
					result[column] = Math.Max(result[column], _content[row, column].Length);
				}
			}
			return result;
		}
	}
}
