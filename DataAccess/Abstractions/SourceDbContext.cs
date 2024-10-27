using fleetpanda.dataaccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace fleetpanda.dataaccess.Abstractions
{
    public class SourceDbContext(DbContextOptions<SourceDbContext> options):DbContext(options)
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
    }

    public class TargetDbContext(DbContextOptions<TargetDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<JobSettings> JobSettings { get; set; }
    }
}
