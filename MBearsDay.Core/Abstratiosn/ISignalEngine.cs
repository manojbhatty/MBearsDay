using MBearsDay.Core.Models;

namespace MBearsDay.Core.Abstractions;

public interface ISignalEngine
{
    Task<TradeDecision> GenerateSignalAsync(string ticker, Position? position = null);
    Task<TradeSignal> AnalyzeAsync(string ticker, CancellationToken ct);
}