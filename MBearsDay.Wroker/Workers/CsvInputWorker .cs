using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MBearsDay.Core.Abstractions;

namespace MBearsDay.Worker.Input;

public class CsvInputWorker : BackgroundService
{
    private readonly IOrchestrator _orchestrator;
    private readonly ILogger<CsvInputWorker> _logger;
    private readonly string _filePath;

    public CsvInputWorker(
        IOrchestrator orchestrator,
        ILogger<CsvInputWorker> logger,
        IConfiguration config)
    {
        _orchestrator = orchestrator;
        _logger = logger;

        _filePath = config["Input:CsvPath"] ?? "tickers.csv";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CSV input worker started using {file}", _filePath);

        if (!File.Exists(_filePath))
        {
            _logger.LogWarning("CSV file not found: {file}", _filePath);
            return;
        }

        var tickers = await File.ReadAllLinesAsync(_filePath, stoppingToken);

        foreach (var raw in tickers)
        {
            if (stoppingToken.IsCancellationRequested)
                break;

            var ticker = raw.Trim();

            if (string.IsNullOrWhiteSpace(ticker))
                continue;

            try
            {
                _logger.LogInformation("Processing ticker from CSV: {ticker}", ticker);

                await _orchestrator.RunSingleTickerAsync(ticker, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing CSV ticker {ticker}", ticker);
            }
        }

        _logger.LogInformation("CSV input worker finished");
    }
}