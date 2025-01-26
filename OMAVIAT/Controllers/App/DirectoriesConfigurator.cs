namespace OMAVIAT.Controllers.App;

public class DirectoriesConfigurator
{
	public static Task Create()
	{
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "news"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", "b1"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", "b2"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", "b3"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", "b4"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "workers"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "people"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", "latest"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "journal"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "pay"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "static"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "static", "documents"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "bitrix"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", "b1"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", "b2"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", "b3"));
		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", "b4"));

		Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Logs"));
		return Task.CompletedTask;
	}
}