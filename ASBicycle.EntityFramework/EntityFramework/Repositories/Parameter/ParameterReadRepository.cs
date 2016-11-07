using Abp.EntityFramework;
using ASBicycle.Parameter;

namespace ASBicycle.EntityFramework.Repositories.Parameter
{
    public class ParameterReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Parameter, int>, IParameterReadRepository
    {
        public ParameterReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}