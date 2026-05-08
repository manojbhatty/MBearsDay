using System.Text.Json.Serialization;

namespace MBearsDay.Infrastructure.Telegram.Dto;

public class TelegramMessageDto
{
    [JsonPropertyName("message_id")]
    public long MessageId { get; set; }

    [JsonPropertyName("chat")]
    public TelegramChatDto Chat { get; set; } = new();

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
