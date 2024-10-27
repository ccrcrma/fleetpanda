using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fleetpanda.dataaccess.Entities;
[Table("SyncHistory")]
public class SyncHistory
{
    [Key]
    public long SyncHistoryId { get; set; }
    public DateTime TimeStamp { get; set; }
    public required string Log { get; set; }
}
