using fleetpanda.dataaccess.Abstractions;

namespace fleetpanda.dataaccess.Repositories.Abstractions
{
    public interface ICustomerRepository
    {
        Task<common.Response> GetCustomersAtSourceAsync();
        Task<common.Response> GetCustomersAtTargetAsync();
        Task<common.Response> SyncCustomersToTargetAsync();
        Task<common.Response> GetCustomerLocationsAsync();
    }
}
