using Abp.EntityFramework;
using ASBicycle.Refound;

namespace ASBicycle.EntityFramework.Repositories.Refound
{
    public class RefoundReadRepositroy : ReadonlyASBicycleRepositoryBase<Entities.Refound, int>, IRefoundReadRepository
    {
        public RefoundReadRepositroy(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}