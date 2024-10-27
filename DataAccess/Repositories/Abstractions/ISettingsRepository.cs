using fleetpanda.dataaccess.Entities;
namespace fleetpanda.dataaccess.Repositories.Abstractions;
public interface ISettingsRepository
{
    Task<common.Response> AddJobSettingsAsync(JobSettings model);

    Task<common.Response> GetActiveJobSetting();
}
