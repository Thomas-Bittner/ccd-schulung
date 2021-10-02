using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvTable
{
	public class CsvTableCreator
	{
		public IEnumerable<string> Header { get; set; }
		public IEnumerable<IEnumerable<string>> Content { get; set; }

		public static string Tabellieren(string csvInput)
		{
			var csvTableCreator = FromCsv(csvInput);
			return csvTableCreator.ToString();
		}

		public static CsvTableCreator FromCsv(string csv)
		{
			var lines = SplitCsvIntoLines(csv);
			return new CsvTableCreator
			{
				Header = ExtractHeader(lines[0]),
				Content = ExtractContent(lines.Skip(1))
			};
		}

		private static string[] SplitCsvIntoLines(string csv)
			=> csv.Split('\n');

		private static IEnumerable<string> ExtractHeader(string header)
			=> header.Split(';');

		private static IEnumerable<IEnumerable<string>> ExtractContent(IEnumerable<string> content)
		{
			var result = content.Select(line => line.Split(';'));
			result = result.SkipLast(1);
			return result;
		}

		public override string ToString()
		{
			var columnWidths = CalculateColumnWidths();
			return ToString(columnWidths);
		}

		private IEnumerable<int> CalculateColumnWidths()
		{
			var result = CalculateColumnWidthsForLine(Header);
			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var row in Content)
			{
				var columnWidths = CalculateColumnWidthsForLine(row);
				result = result.Zip(columnWidths, Math.Max);
			}
			return result;
		}

		private static IEnumerable<int> CalculateColumnWidthsForLine(IEnumerable<string> line)
			=> line.Select(entry => entry.Length);

		public string ToString(IEnumerable<int> columnWidths)
		{
			// ReSharper disable PossibleMultipleEnumeration
			var headerLine = ToReadableLine(Header, columnWidths);
			var borderLine = CreateBorderLine(columnWidths);
			var content = CreateContentBlock(columnWidths);
			return ConcatenateLines(new List<string> {headerLine, borderLine, content});
		}

		private static string CreateBorderLine(IEnumerable<int> columnWidths)
		{
			var minuses = columnWidths.Select(width => new string('-', width));
			return string.Join("+", minuses) + '+';
		}

		private string CreateContentBlock(IEnumerable<int> columnWidths)
		{
			var contentLines = Content.Select(line => ToReadableLine(line, columnWidths));
			return string.Join('\n', contentLines);
		}

		private static string ToReadableLine(IEnumerable<string> lineContents, IEnumerable<int> columnWidths)
		{
			var paddedLineContents = lineContents.Zip(columnWidths, (entry, width) => entry.PadRight(width));
			return string.Join("|", paddedLineContents) + '|';
		}

		private static string ConcatenateLines(IEnumerable<string> lines)
			=> string.Join('\n', lines) + '\n';
	}
}
