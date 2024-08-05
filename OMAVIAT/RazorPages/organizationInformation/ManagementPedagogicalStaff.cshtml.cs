using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation
{
	public class ManagementPedagogicalStaffModel : PageModel
	{
		private readonly ILogger<ManagementPedagogicalStaffModel> _logger;

		public ManagementPedagogicalStaffModel(ILogger<ManagementPedagogicalStaffModel> logger)
		{
			_logger = logger;
		}

		public int id { get; set; }
		public void OnGet(int? id)
		{

			this.id = id - 1 ?? 0;
		}
	}
}