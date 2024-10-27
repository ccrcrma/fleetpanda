using fleetpanda.common;
using fleetpanda.dataaccess.Abstractions;
using fleetpanda.dataaccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using fleetpanda.dataaccess.Entities;

namespace fleetpanda.dataaccess.Repositories
{
    public class SettingsRepository : Repository, ISettingsRepository
    {
        public SettingsRepository(SourceDbContext db, TargetDbContext targetdb) : base(db, targetdb)
        {
        }

        public async Task<Response> AddJobSettingsAsync(JobSettings model)
        {
            var currentSettings = await Targetdb.JobSettings.FirstOrDefaultAsync(s => s.IsActive == true);
            if (currentSettings is not null) { 
                currentSettings.IsActive = false;
                // disable current 
            }
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            await Targetdb.JobSettings.AddAsync(model);
            await Targetdb.SaveChangesAsync();
            return Success("settings updated Successfully", model);
        }

        public async Task<Response> GetActiveJobSetting()
        {
            var currentSettings = await Targetdb.JobSettings.FirstOrDefaultAsync(s => s.IsActive);
            return Success("Data Fetched Successfully", currentSettings);

        }
    }
}
