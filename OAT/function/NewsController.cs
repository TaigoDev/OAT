using System.ComponentModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace OAT.function
{
    public class NewsController
    {
        public static List<NewsData> News = new List<NewsData>();
        public static List<List<NewsController.NewsData>> pages = new List<List<NewsController.NewsData>>();

        public static void init()
        {
            if (!Directory.Exists("news"))
                Directory.CreateDirectory("news");
            if (!File.Exists("news/example.txt"))
                CreateExample();
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
            while (true)
            {

                string[] files = System.IO.Directory.GetFiles("news", "*.yaml");
                News.Clear();
                foreach (string file in files)
                    try
                    {
                        var news = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build().Deserialize<NewsDataYML>(File.ReadAllText(file));
                        News.Add(new NewsData
                        {
                            id = int.Parse(Path.GetFileName(file).Replace(".yaml", "")),
                            short_description = string.Join(" ", news.text.GetWords(30)),
                            data = news,
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"OAT.Core.News: File upload error {Path.GetFileName(file)}");
                    }

                Console.WriteLine($"OAT.Core.News: We successful load updated news");
                News = News.OrderBy(x => x.id).ToList();
                News.Reverse();
                pages = new List<List<NewsData>>();
                var newsOnPage = new List<NewsController.NewsData>();
                for (int i = 0; i < NewsController.News.Count; i++)
                {
                    newsOnPage.Add(NewsController.News[i]);
                    if ((i % 10 == 0 && i != 0) || (News.Count < 10 && i + 1 == News.Count))
                    {
                        pages.Add(newsOnPage);
                        newsOnPage = new List<NewsController.NewsData>();
                    }
                }
                Thread.Sleep(30000);
            }
        }).Start();


        public class NewsData
        {
            public int id { get; set; }
            public string short_description { get; set; }
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

            public NewsDataYML(string date, string title, string text, List<string> photos)
            {
                this.date = date;
                this.title = title;
                this.text = text;
                this.photos = photos;
            }
            public NewsDataYML() { }

        }
    }
}
