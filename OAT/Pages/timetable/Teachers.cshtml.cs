using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Readers;
using System.Text.RegularExpressions;

namespace OAT.Pages.timetable
{
    public class TeachersModel : PageModel
    {
        public string building;
        public void OnGet(string building)
        {
            this.building = building;

        }
    }
}
