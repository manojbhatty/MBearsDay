using MBearsDay.Core.Abstractions;
using MBearsDay.Services;
using MBearsDay.Services.Orchestration;
using MBearsDay.Services.Risk;
using MBearsDay.Services.Signals;
using MBearsDay.Services.Tickers;

namespace MBearsDay.Wroker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddSingleton<IOrchestrator, Orchestrator>();
            builder.Services.AddSingleton<ISignalEngine, SignalEngine>();
            builder.Services.AddSingleton<ITickerDiscovery, TickerDiscovery>();
            builder.Services.AddSingleton<IRiskManager, RiskManager>();
            var host = builder.Build();
            host.Run();
        }
    }
}
