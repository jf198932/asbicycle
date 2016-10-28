using Abp.Domain.Repositories;

namespace ASBicycle.Log
{
    public interface ILogReadRepository : IRepository<Entities.Log, int>
    {
         
    }
}