using Abp.EntityFramework;
using ASBicycle.Refound;

namespace ASBicycle.EntityFramework.Repositories.Refound
{
    public class RefoundWriteRepository : ASBicycleRepositoryBase<Entities.Refound, int>, IRefoundWriteRepository
    {
        public RefoundWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}