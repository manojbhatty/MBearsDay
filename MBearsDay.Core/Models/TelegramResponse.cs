namespace MBearsDay.Core.Models;

public class TelegramResponse
{
    public string CandidateId { get; set; } = string.Empty;

    public bool Approved { get; set; }

    public DateTime RespondedAt { get; set; } = DateTime.UtcNow;
}