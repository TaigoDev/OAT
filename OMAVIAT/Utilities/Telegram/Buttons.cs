using Telegram.Bot.Types.ReplyMarkups;

namespace OMAVIAT.Utilities.Telegram
{
	public class Buttons
	{

		public static InlineKeyboardButton CreateButton(string text, string cmd) =>
			InlineKeyboardButton.WithCallbackData(text, cmd);

		public static InlineKeyboardButton CreateButtonWithUrl(string text, string url) =>
			InlineKeyboardButton.WithUrl(text, url);

		public static InlineKeyboardButton[] CreateButtonInRow(string text, string cmd) =>
			CreateRowButtons(CreateButton(text, cmd));

		public static InlineKeyboardButton[] CreateRowButtons(params InlineKeyboardButton[] cmds) =>
			cmds;

		public static InlineKeyboardMarkup CreateKeyboard(params InlineKeyboardButton[][] rows)
		{
			var buttons = new List<InlineKeyboardButton[]>();
			buttons.AddRange(rows);
			return new InlineKeyboardMarkup(buttons.ToArray());
		}

	}
}
