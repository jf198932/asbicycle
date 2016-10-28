using Abp.Domain.Repositories;

namespace ASBicycle.Log
{
    public interface ILogWriteRepository : IRepository<Entities.Log, int>
    {
         
    }
}