using MBearsDay.Services.Runtime;
using Microsoft.Extensions.Hosting;

public class KeyboardControlWorker : BackgroundService
{
    private readonly TradingControl _control;

    public KeyboardControlWorker(TradingControl control)
    {
        _control = control;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.P)
                    _control.Toggle();

                if (key.Key == ConsoleKey.R)
                    _control.Resume();
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }
}