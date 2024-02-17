#pragma warning disable CS8604
using HtmlAgilityPack;
using System.Text;

public static class ProxyController
{
	public static Config config = new Config();

	public static void BitrixProxy(this WebApplication context) =>
		context.Use((context, next) => Proxy(context, next));

	async static Task Proxy(HttpContext context, Func<Task> next)
	{
		try
		{

			var OnNewSite = UrlsContoller.Redirect(context.Request.Path.Value!);
			if (OnNewSite != null && $"/{OnNewSite}" != context.Request.Path.Value!)
			{
				context.Response.Redirect($"{config.MainUrl}/{OnNewSite}");
				return;
			}
			await next();
			if (context.Response.StatusCode == 404)
				await context.DisplayBitrix(next);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			if (!context.Response.HasStarted)
				context.Response.Redirect("https://www.oat.ru/");
		}
	}

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
		{
			if (path.Contains("students/perfomance") && !string.IsNullOrEmpty(context.Request.Headers["Authorization"].ToString()))
				client.DefaultRequestHeaders.Add("Authorization", context.Request.Headers["Authorization"].ToString());
			response = await client.GetAsync(BaseUrl);
		}
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

		if (path.Contains("students/perfomance") && string.IsNullOrEmpty(context.Request.Headers["Authorization"].ToString()))
		{
			context.Response.Headers.Add("Www-Authenticate", "Basic realm=\"Enter your credentials in domain oat.local\"");
			context.Response.Headers.Add("Vary", "Accept-Encoding");
			context.Response.Headers.Add("Strict-Transport-Security", "max-age=2592000; includeSubDomains; preload");
		}

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

