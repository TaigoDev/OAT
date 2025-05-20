using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another;

public class VeteransSPOModel : PageModel
{
	private readonly ILogger<VeteransSPOModel> _logger;

	public VeteransSPOModel(ILogger<VeteransSPOModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}