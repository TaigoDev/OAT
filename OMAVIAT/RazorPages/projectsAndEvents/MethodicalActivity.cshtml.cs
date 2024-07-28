using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class MethodicalActivityModel : PageModel
	{
		private readonly ILogger<MethodicalActivityModel> _logger;

		public MethodicalActivityModel(ILogger<MethodicalActivityModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}