using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using OMAVIAT.Utilities;

namespace OMAVIAT.Services.AdminPanel;

public class MysqlController : Controller
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[HttpPost]
	[Route("api/mysql/cmd")]
	[AuthorizeRoles(Role.www_admin)]
	[NoCache]
	public IActionResult SendCmd(string command)
	{
		object answer;
		try
		{
			answer = RawSqlQuery(command);
		}
		catch (Exception ex)
		{
			answer = ex.Message;
		}

		Logger.Info(
			$"Пользователь {User.GetUsername()} выполним команду в базе данных - {command}\nIP: {HttpContext.UserIP()}");
		return Ok(answer);
	}

	private static List<string> RawSqlQuery(string query)
	{
		using var context = new DatabaseContext();
		using var command = context.Database.GetDbConnection().CreateCommand();
		command.CommandText = query;
		command.CommandType = CommandType.Text;
		context.Database.OpenConnection();

		using var result = command.ExecuteReader();
		var entities = new List<string>();

		while (result.Read())
		{
			var fieldValues = new Dictionary<string, object>();
			for (var i = 0; i < result.FieldCount; i++)
				fieldValues.Add(result.GetName(i), result[i]);

			entities.Add(fieldValues.toJson());
		}

		return entities;
	}
}