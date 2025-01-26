using Telegram.Bot.Types.ReplyMarkups;

namespace OMAVIAT.Utilities.Telegram;

public class Buttons
{
	public static InlineKeyboardButton CreateButton(string text, string cmd)
	{
		return InlineKeyboardButton.WithCallbackData(text, cmd);
	}

	public static InlineKeyboardButton CreateButtonWithUrl(string text, string url)
	{
		return InlineKeyboardButton.WithUrl(text, url);
	}

	public static InlineKeyboardButton[] CreateButtonInRow(string text, string cmd)
	{
		return CreateRowButtons(CreateButton(text, cmd));
	}

	public static InlineKeyboardButton[] CreateRowButtons(params InlineKeyboardButton[] cmds)
	{
		return cmds;
	}

	public static InlineKeyboardMarkup CreateKeyboard(params InlineKeyboardButton[][] rows)
	{
		var buttons = new List<InlineKeyboardButton[]>();
		buttons.AddRange(rows);
		return new InlineKeyboardMarkup(buttons.ToArray());
	}
}