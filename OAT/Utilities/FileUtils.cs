namespace OAT.Utilities
{
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
                File.WriteAllText($"{filename}", StringUtils.SerializeYML(obj));
            return StringUtils.DeserializeYML<T>(await File.ReadAllTextAsync($"{filename}"));
        }

        public static bool IsFileLocked(string xlsx)
        {
            try
            {
                using FileStream stream = new FileStream(xlsx, FileMode.Open, FileAccess.Read);
                stream.Close();
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
    }

}
