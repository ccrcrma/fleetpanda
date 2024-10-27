using System.ComponentModel.DataAnnotations;

namespace fleetpanda.webui.Dtos
{
    public class JobSettingsDto
    {
        [Display(Name ="Sync Duration In Minutes")]
        public int DurationInMinutes { get; set; }
        public string Description { get; set; } = null!;
    }
}
