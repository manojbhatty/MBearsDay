public class Message
{
    public long MessageId { get; set; }
    public Chat Chat { get; set; } = new();
    public string? Text { get; set; }
}