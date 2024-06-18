namespace OAT.Utilities
{
	public class Translit
	{
		private static Dictionary<string, string> dictionaryChar = new()
		{ 
		  {"а","a"}, {"б","b"}, {"в","v"}, {"г","g"}, {"д","d"}, {"е","e"}, {"ё","yo"},
		  {"ж","zh"}, {"з","z"}, {"и","i"}, {"й","y"}, {"к","k"}, {"л","l"},  {"м","m"},
		  {"н","n"}, {"о","o"}, {"п","p"}, {"р","r"}, {"с","s"}, {"т","t"}, {"у","u"},
		  {"ф","f"}, {"х","h"}, {"ц","ts"}, {"ч","ch"}, {"ш","sh"}, {"щ","sch"}, {"ъ","'"},
          {"ы","yi"}, {"ь",""}, {"э","e"}, {"ю","yu"}, {"я","ya"},

          {"А","A"}, {"Б","B"}, {"В","V"}, {"Г","G"}, {"Д","D"}, {"Е","E"}, {"Ё","YO"},
		  {"Ж","ZH"}, {"З","Z"}, {"И","I"}, {"Й","Y"}, {"К","K"}, {"Л","L"},  {"М","M"},
		  {"Н","N"}, {"О","O"}, {"П","P"}, {"Р","R"}, {"С","S"}, {"Т","T"}, {"У","U"},
		  {"Ф","F"}, {"Х","H"}, {"Ц","TS"}, {"Ч","CH"}, {"Ш","SH"}, {"Щ","SCH"}, {"Ъ","'"},
		  {"Ы","YI"}, {"Ь",""}, {"Э","E"}, {"Ю","YU"}, {"Я","YA"},
		};

		public static string TranslitRusToEn(string source)
		{
			var result = string.Empty;
			foreach (var ch in source)
				result += dictionaryChar.TryGetValue(ch.ToString(), out var ss) ? ss : ch.ToString();
			return result;
		}
		
		public static string TranslitEnToRus(string? source)
		{
			if(source is null)
				return string.Empty;
			var result = string.Empty;
			var dict = dictionaryChar.ToDictionary(x => x.Value, x => x.Key);
			foreach (var ch in source)
				result += dict.TryGetValue(ch.ToString(), out var ss) ? ss : ch.ToString();
			return result;
		}

	}
}
