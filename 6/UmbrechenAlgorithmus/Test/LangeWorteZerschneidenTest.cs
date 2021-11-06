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
			var maxZeilenl�nge = 1;

			var result = input.Substring(0, maxZeilenl�nge);

			result.Should().Be("x");
		}

		[Fact]
		public void Test3()
		{
			var input = "xy";
			var maxZeilenl�nge = 1;

			var result = new string[] { input.Substring(0, maxZeilenl�nge), input.Substring(maxZeilenl�nge, maxZeilenl�nge) };

			result.Should().Equal(new string[] { "x", "y" });
		}

		[Fact]
		public void Test4()
		{
			var input = "wxyz";
			var maxZeilenl�nge = 2;

			var result = new string[] { input.Substring(0, maxZeilenl�nge), input.Substring(maxZeilenl�nge, maxZeilenl�nge) };

			result.Should().Equal(new string[] { "wx", "yz" });
		}

		[Fact]
		public void Test6()
		{
			var input = "uvwxyz";
			var maxZeilenl�nge = 2;

			var result = new List<string> { };
			for (var i = 0; i < input.Length; i += maxZeilenl�nge)
				result.Add(input.Substring(i, maxZeilenl�nge));

			result.Should().Equal(new List<string> { "uv", "wx", "yz" });
		}

		[Fact]
		public void Test7()
		{
			var input = "tuvwxyz";
			var maxZeilenl�nge = 2;

			var result = new List<string> { };
			for (var i = 0; i < input.Length; i += maxZeilenl�nge)
			{
				var schnippelL�nge = Math.Min(input.Length - i, maxZeilenl�nge);
				result.Add(input.Substring(i, schnippelL�nge));
			}

			result.Should().Equal(new List<string> { "tu", "vw", "xy", "z" });
		}

		[Fact]
		public void Test8()
		{
			var input = new List<string> { "abcdefg", "tuvwxyz" };
			var maxZeilenl�nge = 2;

			var result = new List<string> { };
			foreach (var word in input)
			{
				for (var i = 0; i < word.Length; i += maxZeilenl�nge)
				{
					var schnippelL�nge = Math.Min(word.Length - i, maxZeilenl�nge);
					result.Add(word.Substring(i, schnippelL�nge));
				}
			}

			result.Should().Equal(new List<string> { "ab", "cd", "ef", "g", "tu", "vw", "xy", "z" });
		}

		[Fact]
		public void Test9()
		{
			var input = new string[] { "123", "1234", "12345", "123456789" };
			var maxZeilenl�nge = 4;

			var result = LangeWorteZerschneiden(input, maxZeilenl�nge);

			result.Should().Equal(new string[] { "123", "1234", "1234", "5", "1234", "5678", "9" });
		}

		private string[] LangeWorteZerschneiden(string[] worte, int maxZeilenl�nge)
		{
			var schnipselProWort = worte.Select(wort => ExtrahiereSchnipsel(wort, maxZeilenl�nge));
			return schnipselProWort.SelectMany(schnipsel => schnipsel).ToArray();
		}

		private static IEnumerable<string> ExtrahiereSchnipsel(string wort, int maxZeilenl�nge)
		{
			var result = new List<string>();
			for (var i = 0; i < wort.Length; i += maxZeilenl�nge)
			{
				var schnipsel = ExtrahiereSchnipsel(wort, i, maxZeilenl�nge);
				result.Add(schnipsel);
			}
			return result;
		}

		private static string ExtrahiereSchnipsel(string wort, int index, int maxZeilenl�nge)
		{
			var schnippelL�nge = Math.Min(wort.Length - index, maxZeilenl�nge);
			var schnippelSt�ck = wort.Substring(index, schnippelL�nge);
			return schnippelSt�ck;
		}
	}
}
