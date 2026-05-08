public class TelegramUpdate
{
    public long UpdateId { get; set; }
    public string? CallbackData { get; set; }
    public string? CallbackQueryId { get; set; }
    public long? ChatId { get; set; }
    public string? Text { get; set; }
}