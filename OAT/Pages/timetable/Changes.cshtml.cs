using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Readers;

namespace OAT.Pages
{
    [NoCache]
    public class ChangesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ChangesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<Changes>? changes { get; set; }
        public string? sheet { get; set; }
        public int? corpus { get; set; }

        public void OnGet(int? corpus, string? sheet)
        {
            if(corpus is null)
                return;
            

            this.corpus = corpus;
            changes = ChangesController.GetListChanges((int)corpus!);

            if (!changes.Any())
                return;
            
            this.sheet = sheet ?? changes.Last().SheetName;
        }
    }
}