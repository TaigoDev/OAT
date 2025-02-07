using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another.museum;

public class HistoryModel : PageModel
{
	private readonly ILogger<HistoryModel> _logger;

	public HistoryModel(ILogger<HistoryModel> logger)
	{
		_logger = logger;
	}

	public int Id { get; set; }

	public void OnGet(int? id)
	{
		this.Id = id - 1 ?? 0;
	}
}