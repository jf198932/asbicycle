using Abp.EntityFramework;
using ASBicycle.Recharge;

namespace ASBicycle.EntityFramework.Repositories.Recharge
{
    public class RechargeWriteRepository : ASBicycleRepositoryBase<Entities.Recharge, int>, IRechargeWriteRepository
    {
        public RechargeWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}