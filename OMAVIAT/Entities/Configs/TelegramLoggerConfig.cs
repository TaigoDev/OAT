namespace OMAVIAT.Entities.Configs;

public class TelegramLoggerConfig
{
	public string ApiKey { get; set; } = string.Empty;
	public long LogChatId { get; set; }
	public long NewsChannelId { get; set; }
	public long NewsEloChannelId { get; set; }
}