using MBearsDay.Core.Enums;

namespace MBearsDay.Core.Abstractions;

public interface IExecutionGate
{
    TradingState CurrentState { get; }

    void SetAuto();
    void SetManual();
    void Pause();
    void Resume();
    void Stop();

    Task RunIfAllowedAsync(Func<Task> action, CancellationToken ct);
}