namespace OMAVIAT.Utilities;

public class FileUtils
{
	public static void FileDelete(string? path)
	{
		if (File.Exists(path))
			File.Delete(path);
	}

	public static async Task<T> SetupConfiguration<T>(string filename, T obj)
	{
		if (!File.Exists($"{filename}"))
			File.WriteAllText($"{filename}", obj.SerializeYML());
		return (await File.ReadAllTextAsync($"{filename}")).DeserializeYML<T>();
	}

	public static bool IsFileLocked(string xlsx)
	{
		try
		{
			using var stream = new FileStream(xlsx, FileMode.Open, FileAccess.Read);
			stream.Close();
		}
		catch (IOException)
		{
			return true;
		}

		return false;
	}
}