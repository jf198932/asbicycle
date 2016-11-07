using Abp.EntityFramework;
using ASBicycle.Recharge;

namespace ASBicycle.EntityFramework.Repositories.Recharge
{
    public class RechargeReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Recharge, int>, IRechargeReadRepository
    {
        public RechargeReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}