namespace MBearsDay.Core.Abstractions;

public interface IOrchestrator
{
    Task RunCycleAsync(CancellationToken cancellationToken);
    Task RunSingleTickerAsync(string ticker, CancellationToken ct);
}