namespace MBearsDay.Core.Models;

public class ExtendedHoursOptions
{
    public bool Enabled            { get; private set; }
    public bool TimeGatingDisabled { get; private set; }

    public void Enable()           => Enabled = true;
    public void Disable()          => Enabled = false;
    public void DisableTimeGating() => TimeGatingDisabled = true;
    public void EnableTimeGating()  => TimeGatingDisabled = false;
}
