using MBearsDay.Core.Abstractions;
using MBearsDay.Core.Models;
using MBearsDay.Core.Enums;
using Microsoft.Extensions.Configuration;

namespace MBearsDay.CLI.Services;

public class TelegramDiagnosticsService
{
    private readonly ITelegramService _telegram;
    private readonly IConfiguration _config;

    public TelegramDiagnosticsService(ITelegramService telegram, IConfiguration config)
    {
        _telegram = telegram;
        _config = config;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            Console.WriteLine("\n=== TELEGRAM ===");
            Console.WriteLine($"Bot token : {MaskToken(_config["Telegram:BotToken"])}");
            Console.WriteLine($"Chat ID   : {_config["Telegram:ChatId"]}");
            Console.WriteLine();
            Console.WriteLine("1 - Ping (plain text, no response expected)");
            Console.WriteLine("2 - Send approval alert + wait for response");
            Console.WriteLine("3 - Poll updates (single pass)");
            Console.WriteLine("back - return to main menu");
            Console.Write("> ");

            var input = Console.ReadLine()?.Trim();

            if (input == "back") break;

            switch (input)
            {
                case "1": await SendTestMessageAsync(); break;
                case "2": await SendTradeAlertAsync(ct); break;
                case "3": await PollUpdatesAsync(ct); break;
            }
        }
    }

    private async Task SendTestMessageAsync()
    {
        try
        {
            await _telegram.SendMessageAsync("MBearsDay ping — bot is connected.");
            Console.WriteLine("Ping sent. You should see a plain message in Telegram.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task SendTradeAlertAsync(CancellationToken ct)
    {
        try
        {
            Console.Write("Ticker (default AAPL): ");
            var ticker = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(ticker)) ticker = "AAPL";

            var candidate = new TradeCandidate
            {
                Id = Guid.NewGuid().ToString(),
                Ticker = ticker,
                Decision = new TradeDecision
                {
                    Action = TradeAction.BUY,
                    Confidence = 0.82,
                    Reason = "Diagnostics test alert — ignore"
                }
            };

            await _telegram.SendTradeAlertAsync(candidate);
            Console.WriteLine($"Trade alert sent for {ticker} (id: {candidate.Id}).");
            Console.Write("Waiting up to 30s for your response in Telegram");

            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct, timeout.Token);

            while (!linked.Token.IsCancellationRequested)
            {
                var updates = await _telegram.PollAsync(linked.Token);

                foreach (var update in updates)
                {
                    if (string.IsNullOrWhiteSpace(update.CallbackData)) continue;

                    var parts = update.CallbackData.Split('|');
                    if (parts.Length == 2 && parts[1] == candidate.Id)
                    {
                        if (update.CallbackQueryId is not null)
                            await _telegram.AnswerCallbackQueryAsync(update.CallbackQueryId);

                        Console.WriteLine();
                        Console.WriteLine($"Response received: {parts[0]}");
                        return;
                    }
                }

                Console.Write(".");
                await Task.Delay(1000, linked.Token);
            }

            Console.WriteLine();
            Console.WriteLine(timeout.IsCancellationRequested ? "Timed out (30s)." : "Cancelled.");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task PollUpdatesAsync(CancellationToken ct)
    {
        try
        {
            Console.WriteLine("Polling...");
            var updates = await _telegram.PollAsync(ct);

            if (updates.Count == 0)
            {
                Console.WriteLine("No pending updates.");
                return;
            }

            foreach (var u in updates)
            {
                if (u.CallbackData is not null)
                    Console.WriteLine($"  [{u.UpdateId}] callback_data={u.CallbackData}  chat={u.ChatId}");
                else if (u.Text is not null)
                    Console.WriteLine($"  [{u.UpdateId}] text={u.Text}  chat={u.ChatId}");
                else
                    Console.WriteLine($"  [{u.UpdateId}] (unknown update type)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static string MaskToken(string? token)
    {
        if (string.IsNullOrEmpty(token)) return "(not set)";
        return token.Length > 10 ? token[..6] + "..." + token[^4..] : "***";
    }
}
