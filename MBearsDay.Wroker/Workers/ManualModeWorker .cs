using MBearsDay.Core.Abstractions;
using Microsoft.Extensions.Hosting;

namespace MBearsDay.Worker.Workers;

public class ManualModeWorker : BackgroundService
{
    private readonly IManualTradingController _manual;

    public ManualModeWorker(IManualTradingController manual)
    {
        _manual = manual;
    }

    protected override Task ExecuteAsync(CancellationToken ct)
        => _manual.RunAsync(ct);
}
