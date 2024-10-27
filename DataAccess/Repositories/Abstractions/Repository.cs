using fleetpanda.common;
using fleetpanda.dataaccess.Abstractions;

namespace fleetpanda.dataaccess.Repositories.Abstractions
{
    public abstract class Repository
    {
        private readonly SourceDbContext _sourcdb;
        private readonly TargetDbContext _targetdb;
        public Repository(SourceDbContext db, TargetDbContext targetdb)
        {
            _sourcdb = db;
            _targetdb = targetdb;
        }
        protected SourceDbContext SourceDb => _sourcdb;
        public virtual TargetDbContext Targetdb => _targetdb;

        protected Response Success(string? message, object? data = null)
        {
            return new Response { Success = true, Message = message, Data = data };
        }

        protected Response Success(object? data)
        {
            return new Response { Success = true, Data = data };
        }

        protected Response Fail(string error, object? data = null)
        {
            return new Response { Success = false, Message = error, Data = data };
        }
    }
}
