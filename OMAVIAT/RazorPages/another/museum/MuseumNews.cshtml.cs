using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another.museum;

public class MuseumNews : PageModel
{
	public string? Id { get; set; }
	public string? Type { get; set; }

	public void OnGet(string? id, string? type)
	{
		Id = id;
		Type = type;
	}
}