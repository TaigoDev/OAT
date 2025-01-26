using Microsoft.AspNetCore.Mvc.RazorPages;
using OMAVIAT.Entities.Models;
using OMAVIAT.Utilities;
using static OMAVIAT.Services.Workers.WorkersReader;

namespace OMAVIAT.Pages.organizationInformation;

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
		                                        StringUtils.ConvertFullNameToShortName(e.FullName).Replace(".", "")
			                                        .Replace(" ", "") == FullName.Replace(".", "").Replace(" ", ""));
	}
}