namespace OMAVIAT.Utilities;

public static class IntUtils
{
	public static bool HasDigit(this string? s)
	{
		s ??= string.Empty;
		return int.TryParse(string.Join("", s.Where(char.IsDigit)), null, out var id);
	}

	public static bool GetDigit(this string s, out int id)
	{
		s ??= string.Empty;
		return int.TryParse(string.Join("", s.Where(char.IsDigit)), null, out id);
	}

	public static int GetDigit(this string s)
	{
		s ??= string.Empty;
		return int.Parse(string.Join("", s.Where(char.IsDigit)));
	}

	public static int? FirstDigit(this string? s)
	{
		s ??= string.Empty;
		return int.Parse(s.FirstOrDefault(char.IsDigit).ToString());
	}

	public static int ToInt32(string i)
	{
		if (int.TryParse(i, out var integer))
			return integer;
		return 0;
	}
}