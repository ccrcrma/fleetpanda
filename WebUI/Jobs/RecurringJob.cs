using fleetpanda.dataaccess.Repositories.Abstractions;

namespace fleetpanda.webui.Jobs
{
    public class RecurringJob(ICustomerRepository customerRepository)
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task ExecuteAsync()
        {
            await _customerRepository.SyncCustomersToTargetAsync();
        }
    }
}
