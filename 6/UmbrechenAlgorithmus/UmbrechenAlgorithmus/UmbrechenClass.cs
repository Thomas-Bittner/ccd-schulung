using System;
using System.Collections.Generic;
using System.Linq;

namespace UmbrechenAlgorithmus
{
	public static class UmbrechenClass
	{
		public static string Umbrechen(string text, int maxZeilenlänge)
		{
			var worte = ErstelleWortListe(text);
			string[] zeilen = Formatieren(maxZeilenlänge, worte);
			return FügeZeilenZusammen(zeilen);
		}

		private static string[] Formatieren(int maxZeilenlänge, string[] worte)
		{
			var kurzeWorte = LangeWorteZerschneiden(worte, maxZeilenlänge);
			var zeilen = ErstelleZeilen(kurzeWorte, maxZeilenlänge);
			return zeilen;
		}

		public static string[] LangeWorteZerschneiden(string[] worte, int maxZeilenlänge)
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

		public static string[] ErstelleWortListe(string text)
			=> text.Replace("\n", " ").Split(" ");

		public static string FügeZeilenZusammen(string[] zeilen)
			=> String.Join("\n", zeilen);
	}
}
