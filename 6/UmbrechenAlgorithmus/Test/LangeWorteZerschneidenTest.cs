using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Test
{
	public class LangeWorteZerschneidenTest
	{

		[Fact]
		public void Test2()
		{
			var input = "x";
			var maxZeilenlänge = 1;

			var result = input.Substring(0, maxZeilenlänge);

			result.Should().Be("x");
		}

		[Fact]
		public void Test3()
		{
			var input = "xy";
			var maxZeilenlänge = 1;

			var result = new string[] { input.Substring(0, maxZeilenlänge), input.Substring(maxZeilenlänge, maxZeilenlänge) };

			result.Should().Equal(new string[] { "x", "y" });
		}

		[Fact]
		public void Test4()
		{
			var input = "wxyz";
			var maxZeilenlänge = 2;

			var result = new string[] { input.Substring(0, maxZeilenlänge), input.Substring(maxZeilenlänge, maxZeilenlänge) };

			result.Should().Equal(new string[] { "wx", "yz" });
		}

		[Fact]
		public void Test6()
		{
			var input = "uvwxyz";
			var maxZeilenlänge = 2;

			var result = new List<string> { };
			for (var i = 0; i < input.Length; i += maxZeilenlänge)
				result.Add(input.Substring(i, maxZeilenlänge));

			result.Should().Equal(new List<string> { "uv", "wx", "yz" });
		}

		[Fact]
		public void Test7()
		{
			var input = "tuvwxyz";
			var maxZeilenlänge = 2;

			var result = new List<string> { };
			for (var i = 0; i < input.Length; i += maxZeilenlänge)
			{
				var schnippelLänge = Math.Min(input.Length - i, maxZeilenlänge);
				result.Add(input.Substring(i, schnippelLänge));
			}

			result.Should().Equal(new List<string> { "tu", "vw", "xy", "z" });
		}

		[Fact]
		public void Test8()
		{
			var input = new List<string> { "abcdefg", "tuvwxyz" };
			var maxZeilenlänge = 2;

			var result = new List<string> { };
			foreach (var word in input)
			{
				for (var i = 0; i < word.Length; i += maxZeilenlänge)
				{
					var schnippelLänge = Math.Min(word.Length - i, maxZeilenlänge);
					result.Add(word.Substring(i, schnippelLänge));
				}
			}

			result.Should().Equal(new List<string> { "ab", "cd", "ef", "g", "tu", "vw", "xy", "z" });
		}

		[Fact]
		public void Test9()
		{
			var input = new string[] { "123", "1234", "12345", "123456789" };
			var maxZeilenlänge = 4;

			var result = LangeWorteZerschneiden(input, maxZeilenlänge);

			result.Should().Equal(new string[] { "123", "1234", "1234", "5", "1234", "5678", "9" });
		}

		private string[] LangeWorteZerschneiden(string[] worte, int maxZeilenlänge)
		{
			var schnipselProWort = worte.Select(wort => ExtrahiereSchnipsel(wort, maxZeilenlänge));
			return schnipselProWort.SelectMany(schnipsel => schnipsel).ToArray();
		}

		private static IEnumerable<string> ExtrahiereSchnipsel(string wort, int maxZeilenlänge)
		{
			var result = new List<string>();
			for (var i = 0; i < wort.Length; i += maxZeilenlänge)
			{
				var schnipsel = ExtrahiereSchnipsel(wort, i, maxZeilenlänge);
				result.Add(schnipsel);
			}
			return result;
		}

		private static string ExtrahiereSchnipsel(string wort, int index, int maxZeilenlänge)
		{
			var schnippelLänge = Math.Min(wort.Length - index, maxZeilenlänge);
			var schnippelStück = wort.Substring(index, schnippelLänge);
			return schnippelStück;
		}
	}
}
