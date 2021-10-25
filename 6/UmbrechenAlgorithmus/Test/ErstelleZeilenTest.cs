using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Test
{
	public class ErstelleZeilenTest
	{
		[Fact]
		public void ErstelleZeilen1()
		{
			var input = new string[] { "" };
			var maxZeilenlänge = 10;

			var result = "";
			result.Should().Be("");
		}

		[Fact]
		public void ErstelleZeilen2()
		{
			var input = new string[] { "x" };
			var maxZeilenlänge = 10;

			var result = String.Join("", input);
			result.Should().Be("x");
		}

		[Fact]
		public void ErstelleZeilen3()
		{
			var input = new string[] { "x", "y" };
			var maxZeilenlänge = 10;

			var result = String.Join(" ", input);
			result.Should().Be("x y");
		}

		[Fact]
		public void ErstelleZeilen4()
		{
			var input = new string[] { "xy", "yz" };
			var maxZeilenlänge = 4;

			var result = new List<string>();
			result.Add("xy");
			result.Add("yz");

			result.Should().Equal(new List<string> { "xy", "yz" });
		}

		[Fact]
		public void ErstelleZeilen5()
		{
			var input = new string[] { "a", "xy", "yz" };
			var maxZeilenlänge = 4;

			var result = new List<string>();
			result.Add(input[0]);
			if (result.Last().Length + input[1].Length + 1 <= maxZeilenlänge)
				result[result.Count - 1] = result.Last() + " xy";
			result.Add("yz");

			result.Should().Equal(new List<string> { "a xy", "yz" });
		}

		[Fact]
		public void ErstelleZeilen6()
		{
			var input = new string[] { "a", "xy", "yz", "b" };
			var maxZeilenlänge = 4;

			var result = new List<string> { };
			foreach (var token in input)
			{
				if (result.Count > 0 && result.Last().Length + token.Length + 1 <= maxZeilenlänge)
					result[result.Count - 1] = result.Last() + " " + token;
				else
					result.Add(token);
			}

			result.Should().Equal(new List<string> { "a xy", "yz b" });
		}

		[Fact]
		public void ErstelleZeilen7()
		{
			var input = new string[] { "a", "xy", "yz", "b" };
			var maxZeilenlänge = 4;
			var result = ErstelleZeilen(input, maxZeilenlänge);
			result.Should().Equal(new List<string> { "a xy", "yz b" });
		}

		private static string[] ErstelleZeilen(string[] tokens, int maxZeilenlänge)
		{
			var result = new List<string>();
			foreach (var token in tokens)
			{
				if (result.Count > 0 && result.Last().Length + token.Length + 1 <= maxZeilenlänge)
					result[result.Count - 1] = result.Last() + " " + token;
				else
					result.Add(token);
			}
			return result.ToArray();
		}

	}
}
