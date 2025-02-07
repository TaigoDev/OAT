namespace OMAVIAT.Utilities;

public static class ControllersUtils
{
	public static bool IsCorrectFile(IFormFile file, string expansion)
	{
		if (file is null || file.Length == 0 || Path.GetExtension(file.FileName) != expansion)
			return false;
		return true;
	}

	public static bool IsCorrectFile(IFormFile file, params string[] expansions)
	{
		return expansions.Any(expansion => IsCorrectFile(file, expansion));
	}

	public static string UserIP(this HttpContext context)
	{
		return !string.IsNullOrWhiteSpace(context.Request.Headers["CF-Connecting-IP"])
			? context.Request.Headers["CF-Connecting-IP"].ToString() ?? "localhost"
			: context.Request.Headers["X-Real-IP"].ToString() ?? "localhost";
	}

	public static IApplicationBuilder UseNoSniffHeaders(this IApplicationBuilder builder)
	{
		return builder.Use(async (context, next) =>
		{
			context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
			await next();
		});
	}
}