using System.Collections.Generic;

namespace RomanNumerals
{
	public static class NumberExtensions
	{
		private static readonly Dictionary<int, string> LiteralDictionary = new()
		{
			[1000] = "M",
			[900] = "CM",
			[500] = "D",
			[400] = "CD",
			[100] = "C",
			[90] = "XC",
			[50] = "L",
			[40] = "XL",
			[10] = "X",
			[9] = "IX",
			[5] = "V",
			[4] = "IV",
			[1] = "I"
		};

		public static string ToRoman(this int number)
		{
			var result = "";
			foreach (var (value, romanLiteral) in LiteralDictionary)
			{
				while (number >= value)
				{
					result += romanLiteral;
					number -= value;
				}
			}
			return result;
		}
	}
}
