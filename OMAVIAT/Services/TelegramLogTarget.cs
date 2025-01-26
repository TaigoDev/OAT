using NLog;
using NLog.Targets;
using OMAVIAT.Utilities.Telegram;

namespace OMAVIAT.Services;

[Target("TelegramLogTarget")]
public class TelegramLogTarget : AsyncTaskTarget
{
	protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken token)
	{
		try
		{
			var message = Layout.Render(logEvent);
			await TelegramBot.SendMessage(message);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}
}