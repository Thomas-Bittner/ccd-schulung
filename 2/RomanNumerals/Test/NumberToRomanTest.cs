using RomanNumerals;
using Xunit;

namespace Test
{
	public class NumberToRomanTest
	{
		[Theory]
		[InlineData(0, "")]
		[InlineData(1, "I")]
		[InlineData(5, "V")]
		[InlineData(10, "X")]
		[InlineData(50, "L")]
		[InlineData(100, "C")]
		[InlineData(500, "D")]
		[InlineData(1000, "M")]
		[InlineData(2, "II")]
		[InlineData(20, "XX")]
		[InlineData(200, "CC")]
		[InlineData(2000, "MM")]
		[InlineData(3, "III")]
		[InlineData(30, "XXX")]
		[InlineData(300, "CCC")]
		[InlineData(3000, "MMM")]
		[InlineData(4, "IV")]
		[InlineData(9, "IX")]
		[InlineData(40, "XL")]
		[InlineData(90, "XC")]
		[InlineData(400, "CD")]
		[InlineData(900, "CM")]
		[InlineData(19, "XIX")]
		[InlineData(1984, "MCMLXXXIV")]
		[InlineData(1492, "MCDXCII")]
		public void ToRoman(int number, string expectedResult)
		{
			Assert.Equal(expectedResult, number.ToRoman());
		}
	}
}
