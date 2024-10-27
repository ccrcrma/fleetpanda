using System.ComponentModel.DataAnnotations.Schema;

namespace fleetpanda.dataaccess.Entities;

[Table("Customer")]
public class Customer
{
    public int CustomerId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
}

