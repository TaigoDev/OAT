using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Readers;
using OAT.Utilities;

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

    public class RowChangesHelper
    {
        public static List<string> GetListCSS()
        {
            var show_css_class = new List<string>();
            for (int i = 0; i < new Random().Next(1, 8); i++)
                show_css_class.Add(StringUtils.RandomString(15, true));
            return show_css_class;
        }

        public static List<string> ToCSSList(params List<string>[] strings)
        {
            var css = new List<string>();
            foreach (var s in strings)
                css.AddRange(s);
            return css.OrderBy(x => new Random().Next()).ToList();
        }
    }
}