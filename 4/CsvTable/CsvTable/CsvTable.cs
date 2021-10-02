using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvTable
{
	public class CsvTableCreator
	{
		private readonly IEnumerable<string> _header;
		private readonly IEnumerable<IEnumerable<string>> _content;

		public static string Tabellieren(string csvInput)
		{
			var csvTableCreator = FromCsv(csvInput);
			return csvTableCreator.ToString();
		}

		private static CsvTableCreator FromCsv(string csv)
		{
			var lines = SplitCsvIntoLines(csv);
			var header = ExtractHeader(lines[0]);
			var content = ExtractContent(lines.Skip(1));
			return new CsvTableCreator(header, content);
		}

		private static string[] SplitCsvIntoLines(string csv)
			=> csv.Split('\n');

		public static IEnumerable<string> ExtractHeader(string header)
			=> header.Split(';');

		public static IEnumerable<IEnumerable<string>> ExtractContent(IEnumerable<string> content)
		{
			var result = content.Select(line => line.Split(';'));
			result = result.SkipLast(1);
			return result;
		}

		public CsvTableCreator(IEnumerable<string> header, IEnumerable<IEnumerable<string>> content)
		{
			_header = header;
			_content = content;
		}

		public override string ToString()
		{
			var columnWidths = CalculateColumnWidths();
			return ToString(columnWidths);
		}

		public IEnumerable<int> CalculateColumnWidths()
		{
			var result = _header.Select(title => title.Length);
			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var row in _content)
			{
				var columnWidths = row.Select(entry => entry.Length);
				result = result.Zip(columnWidths, Math.Max);
			}
			return result;
		}

		public string ToString(IEnumerable<int> columnWidths)
		{
			// ReSharper disable PossibleMultipleEnumeration
			var headerLine = ToReadableLine(_header, columnWidths);
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
			var contentLines = _content.Select(line => ToReadableLine(line, columnWidths));
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
