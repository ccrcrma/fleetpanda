using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fleetpanda.dataaccess.Entities;
[Table("Location")]
public class Location
{
    [Key]
    public int LocationId { get; set; }
    public int CustomerId { get; set; }
    public required string Address { get; set; }
}
