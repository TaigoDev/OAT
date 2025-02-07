using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another.museum;

public class EventsModel : PageModel
{
	private readonly ILogger<EventsModel> _logger;

	public EventsModel(ILogger<EventsModel> logger)
	{
		_logger = logger;
	}

	public int Id { get; set; }

	public void OnGet(int? id)
	{
		this.Id = id - 1 ?? 0;
	}
}