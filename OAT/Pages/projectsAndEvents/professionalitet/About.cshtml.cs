﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.projectsAndEvents.demoExam
{
	public class AboutModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public AboutModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}