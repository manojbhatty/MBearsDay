using MBearsDay.Core.Abstractions;

namespace MBearsDay.Worker.Workers;

public class StatusWorker : BackgroundService
{
    private static readonly TimeZoneInfo Et = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    private readonly IStatusReporter _reporter;
    private readonly ILogger<StatusWorker> _logger;
    private readonly int _intervalMinutes;

    public StatusWorker(IStatusReporter reporter, ILogger<StatusWorker> logger, IConfiguration config)
    {
        _reporter = reporter;
        _logger = logger;
        _intervalMinutes = config.GetValue<int>("Trading:StatusIntervalMinutes", 60);
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("StatusWorker started (interval: {Minutes} min)", _intervalMinutes);

        // Wait until the top of the next interval boundary before first update
        await DelayUntilNextBoundaryAsync(ct);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                await _reporter.SendStatusAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StatusWorker: error sending periodic status");
            }

            await Task.Delay(TimeSpan.FromMinutes(_intervalMinutes), ct);
        }
    }

    private async Task DelayUntilNextBoundaryAsync(CancellationToken ct)
    {
        var etNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Et);
        var minutesPastBoundary = etNow.Minute % _intervalMinutes;
        var minutesToWait = minutesPastBoundary == 0
            ? _intervalMinutes
            : _intervalMinutes - minutesPastBoundary;
        var delay = TimeSpan.FromMinutes(minutesToWait) - TimeSpan.FromSeconds(etNow.Second);
        if (delay > TimeSpan.Zero)
            await Task.Delay(delay, ct);
    }
}
