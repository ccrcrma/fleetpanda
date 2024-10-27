using fleetpanda.common;
using fleetpanda.dataaccess.Abstractions;
using fleetpanda.dataaccess.Constants;
using fleetpanda.dataaccess.Entities;
using fleetpanda.dataaccess.Repositories.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace fleetpanda.dataaccess.Repositories
{
    public class CustomerRepository: Repository, ICustomerRepository
    {
        public CustomerRepository(dataaccess.Abstractions.SourceDbContext srcdb, TargetDbContext targetDb):base(srcdb, targetDb) { }

 

        public async Task<Response> GetCustomersAtSourceAsync()
        {
           var data = await SourceDb.Customers.ToListAsync();
            return Success(data);
        }

        public async Task<Response> GetCustomersAtTargetAsync()
        {
            var data = await Targetdb.Customers.ToListAsync();
            return Success(data);
        }

        public async Task<Response> GetCustomerLocationsAsync()
        {
            var data = await Targetdb.Locations.ToListAsync();
            return Success(data);
        }

        public async Task<Response> SyncCustomersToTargetAsync()
        {
            const string PROC_NAME = Procedures.PROC_SYNC_CUSTOMERS;
            const string PARAM_NAME = "@customer_records";
            const string PARAM_TYPENAME = "dbo.CustomerType";
            var sourceResponse = await GetCustomersAtSourceAsync();
            if (!sourceResponse.Success)
                return Fail("failed to synchronize databases");
            var sourceData = sourceResponse.Data as IEnumerable<Customer>;
            if (sourceData is null)
                return Fail("failed to synchronize databases");

            var customerDt = new DataTable();
            customerDt.Columns.Add("CustomerId", typeof(int));
            customerDt.Columns.Add("Name", typeof(string));
            customerDt.Columns.Add("Email", typeof(string));
            customerDt.Columns.Add("Phone", typeof(string));
         
            foreach (var record in sourceData)
            {
                customerDt.Rows.Add(record.CustomerId, record.Name, record.Email, record.Phone);
            }

            var paramTable = new SqlParameter
            {
                ParameterName = PARAM_NAME,
                SqlDbType = SqlDbType.Structured,
                Value = customerDt,
                TypeName = PARAM_TYPENAME
            };

            var rowsAffected =  await Targetdb.Database.ExecuteSqlRawAsync("{0} {1}", PROC_NAME, paramTable);
            return Success("data synchronized successfully", rowsAffected);

        }
    }
}
