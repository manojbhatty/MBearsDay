using MBearsDay.Core.Abstractions;
using MBearsDay.Core.Enums;
using System.Threading;

public class ExecutionGate : IExecutionGate
{
    private readonly SemaphoreSlim _lock = new(1, 1);

    public TradingState CurrentState { get; private set; } = TradingState.AutoRunning;

    public void SetAuto() => CurrentState = TradingState.AutoRunning;

    public void SetManual() => CurrentState = TradingState.ManualMode;

    public void Pause() => CurrentState = TradingState.Paused;

    public void Resume() => CurrentState = TradingState.AutoRunning;

    public void Stop() => CurrentState = TradingState.Stopped;

    public async Task RunIfAllowedAsync(Func<Task> action, CancellationToken ct)
    {
        if (CurrentState == TradingState.Stopped)
            return;

        if (CurrentState == TradingState.Paused)
            return;

        if (CurrentState == TradingState.ManualMode)
            return;

        await _lock.WaitAsync(ct);
        try
        {
            await action();
        }
        finally
        {
            _lock.Release();
        }
    }
}