using Abp.EntityFramework;
using ASBicycle.Recharge_detail;

namespace ASBicycle.EntityFramework.Repositories.Recharge_detail
{
    public class Recharge_detailReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Recharge_detail, int>, IRecharge_detailReadRepository
    {
        public Recharge_detailReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}