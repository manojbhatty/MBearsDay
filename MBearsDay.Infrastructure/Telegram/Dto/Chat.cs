using System.Text.Json.Serialization;

namespace MBearsDay.Infrastructure.Telegram.Dto;

public class TelegramChatDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}
