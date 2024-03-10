using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Utilities;
using static OAT.Modules.Workers.WorkersReader;

namespace OAT.Pages.organizationInformation
{
	public class WorkerModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public WorkerModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public Worker? worker { get; set; }

		public void OnGet(string? FullName)
		{
			worker = AllWorkers.FirstOrDefault(e => e.FullName == FullName ||
						StringUtils.ConvertFullNameToShortName(e.FullName).Replace(".", "").Replace(" ", "") == FullName.Replace(".", "").Replace(" ", ""));

		}
	}
}