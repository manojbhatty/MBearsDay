using System.Text.Json.Serialization;

namespace MBearsDay.Infrastructure.Telegram.Dto;

public class TelegramCallbackQueryDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public string? Data { get; set; }
}
