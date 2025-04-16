
using Telegram.Bot.Types;

namespace OMAVIAT.Entities.Telegram;

public record TelegramMediaGroup(string MediaGroupId, List<TelegramPhoto> Photos);

public record TelegramPhoto(int MessageId, PhotoSize Photo);