#pragma warning disable CS8604
using HtmlAgilityPack;
using System.Text;

public static class ProxyController
{
    public static Config config = new Config();
    public static async Task DisplayBitrix(this HttpContext context, Func<Task> next)
    {
        var path = context.Request.Path.Value;
        var BaseUrl = $"{config.BaseUrl}{path}{context.Request.QueryString.Value}";
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        var client = new HttpClient(clientHandler);

        if (IsImage(path, ".PNG", ".png", ".JPG", ".jpg", ".JPEG", ".jpg", ".PDF", ".pdf", ".jpeg"))
        {
            context.Response.Redirect($"/proxing/images/bitrix?url={path}", true);
            return;
        }

        HttpResponseMessage response = null;
        if (context.Request.Method == "GET")
            response = await client.GetAsync(BaseUrl);
        else if (context.Request.Method == "POST")
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

        if (response == null)
        {
            await context.Response.WriteAsync("FATAL ERROR: response was null", Encoding.UTF8);
            return;
        }
        if (!context.Response.HasStarted)
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
            if (!context.Response.HasStarted)
                context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(doc.DocumentNode.OuterHtml, Encoding.UTF8);
        }
        else await context.Response.WriteAsync(body, Encoding.UTF8);


    }

    private static bool IsImage(string path, params string[] expansions)
    {
        foreach (var expansion in expansions)
            if (path.Contains(expansion))
                return true;
        return false;
    }

}

