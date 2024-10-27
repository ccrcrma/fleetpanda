namespace fleetpanda.dataaccess.Configurations;
public class JobsSettings
{
    public string CronExpr { get; set; } = null!;
    public int RecurringTimeInMinutes { get; set; }
}
