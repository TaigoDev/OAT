using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another.museum;

public class EventsModel : PageModel
{
	private readonly ILogger<EventsModel> _logger;

	public EventsModel(ILogger<EventsModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}