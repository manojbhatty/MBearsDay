namespace MBearsDay.Core.Models;

public class RiskCheckResult
{
    public bool Passed { get; init; }
    public string Reason { get; init; } = string.Empty;

    public static RiskCheckResult Ok() => new() { Passed = true };
    public static RiskCheckResult Fail(string reason) => new() { Passed = false, Reason = reason };
}
