using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAT.Utilities;
using System.Data;

namespace OAT.Controllers.AdminPanel
{
	public class MysqlController : Controller
	{
		[HttpPost, Route("api/mysql/cmd"), AuthorizeRoles(Enums.Role.www_admin), NoCache]
		public async Task<IActionResult> SendCmd(string command)
		{
			var answer = new object();
			try
			{
				answer = RawSqlQuery(command);
			}
			catch (Exception ex)
			{
				answer = ex.Message;
			}
			Logger.Info($"Пользователь {User.GetUsername()} выполним команду в базе данных - {command}\nIP: {HttpContext.UserIP()}");
			return Ok(answer);
		}

		public static List<string> RawSqlQuery(string query)
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
				for (int i = 0; i < result.FieldCount; i++)
					fieldValues.Add(result.GetName(i), result[i]);
			
				entities.Add(fieldValues.toJson());
			}

			return entities;
		}


	}
}
