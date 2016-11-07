using Abp.EntityFramework;
using ASBicycle.Parameter;

namespace ASBicycle.EntityFramework.Repositories.Parameter
{
    public class ParameterWriteRepository : ASBicycleRepositoryBase<Entities.Parameter, int>, IParameterWriteRepository
    {
        public ParameterWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}