using Microsoft.AspNetCore.Mvc.RazorPages;
using static OAT.Readers.WorkersReader;

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
            worker = AllWorkers.FirstOrDefault(e => e.FullName == FullName);

        }
    }
}