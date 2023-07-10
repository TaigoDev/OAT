#pragma warning disable CS8604
using HtmlAgilityPack;
using System.Text;

public static class ProxyController
{
    public static Config config = new Config();

    public static async Task DisplayBitrix(this HttpContext context, Func<Task> next)
    {
        var path = context.Request.Path.Value;
        var BaseUrl = $"{config.BaseUrl}{path}";
        var client = new HttpClient();

        if (IsImage(path, ".PNG", ".png", ".JPG", ".jpg", ".JPEG", ".jpg", ".PDF", ".pdf"))
        {
            context.Response.Redirect($"/proxing/images/bitrix?url={path}", true);
            return;
        }

        HttpResponseMessage response = null;
        if (context.Request.Method == "GET")
            response = await client.GetAsync(BaseUrl);


        if (context.Request.Method == "POST")
        {
            try
            {
                string documentContents;
                using (Stream receiveStream = context.Request.Body)
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    documentContents = readStream.ReadToEnd();
                HttpContent content = new StringContent(documentContents, Encoding.UTF8, "application/json");
                response = await client.PostAsync(BaseUrl, content);
            }
            catch (Exception e)
            {
                await context.Response.WriteAsync(e.Message, Encoding.UTF8);
            }
        }

        if (response != null)
        {
            context.Response.StatusCode = (int)response.StatusCode;
            var body = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(body);
            var head = doc.DocumentNode.SelectSingleNode("/html/head");
            if (head != null)
            {
                string newContent = "<meta charset=\"UTF-8\">";
                HtmlNode newNode = HtmlNode.CreateNode(newContent);
                head.InsertBefore(newNode, head.FirstChild);
                if (path.Contains(".js"))
                    context.Response.ContentType = "text/javascript";
                else context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(doc.DocumentNode.OuterHtml, Encoding.UTF8);
            }
            else
            {
                if (path.Contains(".js"))
                    context.Response.ContentType = "text/javascript";
                await context.Response.WriteAsync(body, Encoding.UTF8); 
            }
        }
    }

    private static bool IsImage(string path, params string[] expansions)
    {
        foreach (var expansion in expansions)
            if (path.Contains(expansion))
                return true;
        return false;
    }
    public class Config
    {
        public string BaseUrl { get; set; }

        public Config(string BaseUrl, string MainUrl, int bind_port)
        {
            this.BaseUrl = BaseUrl;
            this.MainUrl = MainUrl;
            this.bind_port = bind_port;
        }
        public Config() { }
        public string MainUrl { get; set; }
        public int bind_port { get; set; }
    }
}

