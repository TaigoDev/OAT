using Microsoft.AspNetCore.Mvc;
using OAT.Utilities;
using System.Text;

namespace OAT.Components
{
	[NoCache]
	public class FakeChangesController : Controller
	{

		[HttpGet("api/multi/css/{hex}")]
		public IActionResult Get(string hex)
		{
			var json = StringUtils.ConvertHexToString(hex);
			var css = json.toObject<FakeCSS>();

			var style = string.Empty;

			foreach (var _css in css.hide_css)
			{
				if (IsChanceToUse())
					style += $".{_css} {{ display: none; }}\n";
				else if (IsChanceToUse())
				{
					style += $".{_css} {{ display: table-row; }}\n";
					style += $".{_css} {{ display: none; }}\n";
				}
				else if (IsChanceToUse())
					style += $".{_css} {{ visibility: collapse; }}\n";
				else
				{
					style += $".{_css} {{ visibility: collapse; }}\n";
					style += $".{_css} {{ visibility: none; }}\n";
				}
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
		public IActionResult GetTables(string hex)
		{
			var json = StringUtils.ConvertHexToString(hex);
			var css = json.toObject<FakeCSS>();

			var style = string.Empty;

			foreach (var _css in css.hide_css)
			{
				if (IsChanceToUse())
					style += $".{_css} {{ visibility: collapse;; }}\n";
				else if (IsChanceToUse())
				{
					style += $".{_css} {{ visibility: visible; }}\n";
					style += $".{_css} {{ visibility: collapse; }}\n";
				}
				else
					style += $".{_css} {{ position: fixed; height: 0; }}\n";
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

		private bool IsChanceToUse() =>
			new Random().Next(1, 100) % 3 == 0;
	}

	public class FakeCSS
	{
		public FakeCSS(List<string> show_css, List<string> hide_css)
		{
			this.show_css = show_css;
			this.hide_css = hide_css;
		}

		public List<string> show_css { get; set; }
		public List<string> hide_css { get; set; }
	}
}
