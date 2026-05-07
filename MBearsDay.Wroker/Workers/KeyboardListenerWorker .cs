using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MBearsDay.Services.Runtime;

namespace MBearsDay.Worker.Workers;

public class KeyboardListenerWorker : BackgroundService
{
    private readonly TradingModeController _mode;
    private readonly ILogger<KeyboardListenerWorker> _logger;

    public KeyboardListenerWorker(
        TradingModeController mode,
        ILogger<KeyboardListenerWorker> logger)
    {
        _mode = mode;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("Keyboard listener started (press 'P' to toggle mode)");

        while (!ct.IsCancellationRequested)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.P)
                {
                    if (_mode.IsAuto)
                    {
                        _mode.SetManual();
                        _logger.LogInformation("Switched to MANUAL mode");
                    }
                    else
                    {
                        _mode.SetAuto();
                        _logger.LogInformation("Switched to AUTO mode");
                    }
                }
            }

            await Task.Delay(100, ct);
        }
    }
}