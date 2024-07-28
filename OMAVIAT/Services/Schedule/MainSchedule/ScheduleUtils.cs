using System.Text;
using System.Xml;

namespace OMAVIAT.Services.Schedule.MainSchedule
{
	public static class ScheduleUtils
	{
		public static async Task<string?> ReadFileAsync(string path)
		{
			for(var i = 0; i < 4; i++)
				try
				{
					using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
					using var reader_encoding = new StreamReader(fs, CodePagesEncodingProvider.Instance.GetEncoding(1251)!);
					return await reader_encoding.ReadToEndAsync();
				}
				catch(Exception ex)
				{
					await Task.Delay(5000);
					Logger.Error(ex);
				}

			Logger.Warning($"[ScheduleUtils]: ReadFileAsync: Не удалось прочитать файл {path}");
			return null;
		}

		public static string? GetAttributeValue(this XmlNode node, string name)
		{
			var attributes = node.Attributes;
			if (attributes == null) return null;
			return attributes.GetAttributeValue(name);
		}

		public static string? GetAttributeValue(this XmlAttributeCollection node, string name)
		{
			var attribute = node[name];
			if (attribute is null) return null;
			var value = attribute.Value;
			if (value is null) return null;
			return value.ToString();
		}

		public static List<XmlNode> GetChilds(this XmlNode node) =>
			node.ChildNodes.Cast<XmlNode>().ToList();
	}
}
