using Microsoft.AspNetCore.Mvc;
using OAT.Utilities;
using System.Text;

namespace OAT.Controllers.Schedules.Controllers
{
	[NoCache]
	public class FakeChangesController : Controller
	{

		[HttpGet("api/multi/css/{hex}")]
		public IActionResult GetRowStyle(string hex)
		{
			var json = StringUtils.ConvertHexToString(hex);
			var css = json.toObject<FakeCSS>();

			var style = string.Empty;

			foreach (var _css in css.hide_css)
			{
				if (IsChanceToUse())
					style += $$"""
						.{{_css}} {
							position: fixed;
							top: -100vw; 
							height: 0;
							display: none;
						}
						""";
				else if (IsChanceToUse())
				{
					style += $$"""
						.{{_css}} {
							padding: 0px;
							display: none; 
							top: -10000px; 
							margin: none;
						}
						""";
					style += $$"""
						.{{_css}} {
							padding: 0; 
							position: fixed; 
							top: 0; 
							height: 0; 
							margin: 0;
							display: none;
						}
						""";
				}
				else if (IsChanceToUse())
					style += $$"""
						.{{_css}} {
							position: fixed;
							top: 0; 
							height: 0;
							margin: 0;
							display: none;
						}
						""";
				else
					style += $$"""
						.{{_css}} {
							padding: 0; 
							position: fixed; 
							top: -100vw;
							height: 0; 
							margin: 0;
							visibility: hidden
							display: none;
						}
						""";

			}

			foreach (var _css in css.show_css)
			{
				if (IsChanceToUse())
					style += $".{_css} {{ display: table-row; }}\n";
				else if (IsChanceToUse())
					style += $".{_css} {{ display: none; display: table-row; }}\n";
				else
				{
					style += $".{_css} {{ display: none; }}\n";
					style += $".{_css} {{ display: table-row; }}\n";
				}
			}

			return File(Encoding.ASCII.GetBytes(style), "text/css");
		}

		[HttpGet("api/multi/css/tables/{hex}")]
		public IActionResult GetTablesStyle(string hex)
		{
			var json = StringUtils.ConvertHexToString(hex);
			var css = json.toObject<FakeCSS>();

			var style = string.Empty;

			foreach (var _css in css.hide_css)
			{
				if (IsChanceToUse())
					style += $$"""
						.{{_css}} {
							padding: 0; 
							position: fixed; 
							top: -100vw;
							height: 0; 
							margin: 0;
							visibility: hidden
						}
						""";
				else if (IsChanceToUse())
				{
					style += $$"""
						.{{_css}} {
							padding: 0px;
							display: none; 
							top: -10000px; 
							margin: none;
						}
						""";
					style += $$"""
						.{{_css}} {
							padding: 0; 
							position: fixed; 
							top: 0; 
							height: 0; 
							margin: 0;
						}
						""";
				}
				else
					style += $$"""
						.{{_css}} {
							padding: 0; 
							position: fixed; 
							top: 0; 
							height: 0; 
							margin: 0;
						}
						""";
			}

			foreach (var _css in css.show_css)
			{
				if (IsChanceToUse())
					style += $".{_css} {{ visibility: visible; }}\n";
				else if (IsChanceToUse())
					style += $".{_css} {{ visibility: collapse; visibility: visible; }}\n";
				else if (IsChanceToUse())
				{
					style += $".{_css} {{ visibility: collapse; }}\n";
					style += $".{_css} {{ visibility: visible; }}\n";
				}
				else if (IsChanceToUse())
					style += $".{_css} {{ display: none; display: table; }}\n";
				else
				{
					style += $".{_css} {{ display: none; }}\n";
					style += $".{_css} {{ display: table; }}\n";
				}

			}

			return File(Encoding.ASCII.GetBytes(style), "text/css");
		}

		public static List<string> GenerateCSS(int count)
		{
			var css = new List<string>();
			for (var i = 0; i < count; i++)
				css.Add(StringUtils.RandomString(8, true));
			return css;
		}

		private static bool IsChanceToUse() =>
			new Random().Next(1, 100) % 3 == 0;
	}

	public class FakeCSS(List<string> show_css, List<string> hide_css)
	{
		public List<string> show_css { get; set; } = show_css;
		public List<string> hide_css { get; set; } = hide_css;
	}
}
