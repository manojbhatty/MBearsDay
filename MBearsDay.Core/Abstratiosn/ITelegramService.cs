using MBearsDay.Core.Models;

namespace MBearsDay.Core.Abstractions;

public interface ITelegramService
{
    Task SendMessageAsync(string text);
    Task SendTradeAlertAsync(TradeCandidate candidate);

    Task<TelegramResponse?> WaitForResponseAsync(string candidateId, CancellationToken ct);

    //Task<long> ProcessUpdatesAsync(long offset, CancellationToken ct);
    void HandleCallback(string candidateId, bool approved);
    Task AnswerCallbackQueryAsync(string callbackQueryId);
    Task<List<TelegramUpdate>> PollAsync(CancellationToken ct);

    Task<TelegramResponse?> SendAndWaitForDecisionAsync(TradeCandidate candidate, CancellationToken ct);
}