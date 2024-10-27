using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fleetpanda.dataaccess.Entities
{
    [Table("JobSettings")]
    public class JobSettings
    {
        [Key]
        public int JobSettingsId { get; set; }
        public string? Description { get; set; } = null!;
        //public string Regex { get; set; } = null!;

        public int DurationInMinutes { get; set; }
        public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; };
        public bool IsActive { get; set; }
    }
}
