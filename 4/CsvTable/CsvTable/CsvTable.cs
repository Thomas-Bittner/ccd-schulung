using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvTable
{
	public class CsvTableCreator
	{
		private readonly IEnumerable<string> _header;
		private readonly IEnumerable<IEnumerable<string>> _content;

		public static CsvTableCreator FromCsv(string csv)
		{
			var lines = csv.Split('\n');
			var header = ExtractHeader(lines[0]);
			var content = ExtractContent(lines.Skip(1).ToArray());
			return new CsvTableCreator(header, content);
		}

		public static IEnumerable<string> ExtractHeader(string header)
			=> header.Split(';');

		public static IEnumerable<IEnumerable<string>> ExtractContent(IEnumerable<string> content)
			=> content.Select(line => line.Split(';'));

		public CsvTableCreator(IEnumerable<string> header, IEnumerable<IEnumerable<string>> content)
		{
			this._header = header;
			this._content = content;
		}

		public IEnumerable<int> CalculateColumnWidths()
		{
			var result = _header.Select(title => title.Length);
			foreach (var row in _content)
			{
				var columnWidths = row.Select(entry => entry.Length);
				result = result.Zip(columnWidths, Math.Max);
			}
			return result;
		}

		public string ToString(int[] columnWidths)
		{
			var headerLine = ToReadableLine(_header, columnWidths);
			var borderLine = CreateBorderLine(columnWidths);
			var contentLines = _content.Select(line => ToReadableLine(line, columnWidths));
			return string.Join('\n', headerLine, borderLine, string.Join('\n', contentLines));
		}

		private static string ToReadableLine(IEnumerable<string> lineContents, IEnumerable<int> columnWidths)
		{
			var paddedLineContents = lineContents.Zip(columnWidths, (entry, width) => entry.PadRight(width));
			return string.Join("|", paddedLineContents);
		}

		private static string CreateBorderLine(IEnumerable<int> columnWidths)
		{
			var minuses = columnWidths.Select(width => new string('-', width));
			return string.Join("+", minuses);
		}
	}
}
