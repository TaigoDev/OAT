using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Reflection.PortableExecutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;


namespace OAT.function
{
	public class NewsController
	{
		public static List<NewsData> News = new List<NewsData>();
		public static void init()
		{
			if (!Directory.Exists("news"))
				Directory.CreateDirectory("news");
			if (!File.Exists("news/example.txt"))
				CreateExample();

			string[] files = System.IO.Directory.GetFiles("news", "*.yaml");
			foreach (string file in files)
				try
				{
					using StreamReader reader = new StreamReader(file);

					News.Add(new NewsData
					{
						id = int.Parse(Path.GetFileName(file).Replace(".yaml", "")) - 1,
						data = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
						.Build().Deserialize<NewsDataYML>(reader.ReadToEnd()),
					});
				}
				catch (Exception ex)
				{
					Console.WriteLine($"OAT.Core.News: File upload error {Path.GetFileName(file)}");
				}

			News = News.OrderBy(x => x.id).ToList();
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				var location = System.Reflection.Assembly.GetExecutingAssembly().Location;

				if (Directory.Exists($"{Path.GetDirectoryName(location)}\\news"))
					Directory.Delete($"{Path.GetDirectoryName(location)}\\news", true);
				Directory.CreateDirectory($"{Path.GetDirectoryName(location)}\\news");
				CopyFilesRecursively($"{Path.GetDirectoryName(location).Split("\\bin\\")[0]}\\news",
					$"{Path.GetDirectoryName(location)}\\news");
			}
			AutoUpdate();
			Console.WriteLine($"OAT.Core.News: We successful load {News.Count} news");
		}
		private static void CreateExample()
		{
			using (StreamWriter writer = new StreamWriter("news/example.txt", false))
				writer.WriteLine(new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build().Serialize(new NewsDataYML()));
		}

		private static void AutoUpdate() => new Thread(() =>
		{
			int count = System.IO.Directory.GetFiles("news", "*.yaml").Length;
			while (true)
			{
				if (count != System.IO.Directory.GetFiles("news", "*.yaml").Length)
				{
					count = System.IO.Directory.GetFiles("news", "*.yaml").Length;
					string[] files = System.IO.Directory.GetFiles("news", "*.yaml");
					News.Clear();
					foreach (string file in files)
						try
						{
							using StreamReader reader = new StreamReader(file);
							News.Add(new NewsData
							{
								id = int.Parse(Path.GetFileName(file).Replace(".yaml", "")) - 1,
								data = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build().Deserialize<NewsDataYML>(reader.ReadToEnd()),
							});
						}
						catch (Exception ex)
						{
							Console.WriteLine($"OAT.Core.News: File upload error {Path.GetFileName(file)}");
						}
				}
				Console.WriteLine($"OAT.Core.News: We successful load updated news");
				News = News.OrderBy(x => x.id).ToList();
				Thread.Sleep(300000);
			}
		}).Start();
		private static void CopyFilesRecursively(string sourcePath, string targetPath)
		{
			foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
				Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));

			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
				File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
		}

		public class NewsData
		{
			public int id { get; set; }
			public NewsDataYML data { get; set; }
		}

		public class NewsDataYML
		{

			[DefaultValue(true)]
			[YamlMember(Alias = "date", ApplyNamingConventions = false, Description = "Дата публикации новости")]
			public string date { get; set; } = "01.01.2001";

			[DefaultValue(true)]
			[YamlMember(Alias = "title", ApplyNamingConventions = false, Description = "Заголовок новой статьи")]
			public string title { get; set; } = "Новая статья на сайте OAT";

			[DefaultValue(true)]
			[YamlMember(Alias = "text", ApplyNamingConventions = false, Description = "Текст статьи. Используйте Enter, чтобы переместиться на новую строку. Смайлики разрешены")]
			public string text { get; set; } = "Текст новой статьи на сайте OAT";

			[DefaultValue(true)]
			[YamlMember(Alias = "photos", ApplyNamingConventions = false, Description = "Фотографии, можно использовать 3, 6, 9 фото. Пример: images/фото.jpg")]
			public List<string> photos { get; set; } = new List<string>() 
			{
				"images/news/news41.jpg",
				"images/news/news42.jpg",
				"images/news/news43.jpg",
				"images/news/news44.jpg",
			};
		}
	}
}
