namespace OAT.Utilities
{
	public class DirectoryUtils
	{
		public static void CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}

		public static void CreateDirectories(params string[] paths)
		{
			foreach (var path in paths)
				CreateDirectory(path);
		}

		public static void CreateDirectoriesWithCurrentPath(params string[] paths)
		{
			foreach (var path in paths)
				CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), path));
		}
	}
}
