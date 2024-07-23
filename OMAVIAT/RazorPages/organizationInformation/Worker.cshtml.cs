using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Utilities;
using static OAT.Controllers.Workers.WorkersReader;

namespace OAT.Pages.organizationInformation
{
	public class WorkerModel : PageModel
	{
		private readonly ILogger<WorkerModel> _logger;

		public WorkerModel(ILogger<WorkerModel> logger)
		{
			_logger = logger;
		}

		public Worker? worker { get; set; }

		public void OnGet(string? FullName)
		{
			if (FullName is null)
				return;
			worker = AllWorkers.FirstOrDefault(e => e.FullName == FullName ||
						StringUtils.ConvertFullNameToShortName(e.FullName).Replace(".", "").Replace(" ", "") == FullName.Replace(".", "").Replace(" ", ""));

		}
	}
}