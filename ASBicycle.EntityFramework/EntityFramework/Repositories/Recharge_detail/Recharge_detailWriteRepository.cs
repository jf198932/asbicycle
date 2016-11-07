using Abp.EntityFramework;
using ASBicycle.Recharge_detail;

namespace ASBicycle.EntityFramework.Repositories.Recharge_detail
{
    public class Recharge_detailWriteRepository : ASBicycleRepositoryBase<Entities.Recharge_detail, int>, IRecharge_detailWriteRepository
    {
        public Recharge_detailWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}