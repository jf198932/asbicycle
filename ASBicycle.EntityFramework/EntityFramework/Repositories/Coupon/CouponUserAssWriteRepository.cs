using Abp.EntityFramework;
using ASBicycle.Coupon;

namespace ASBicycle.EntityFramework.Repositories.Coupon
{
    public class CouponUserAssWriteRepository : ASBicycleRepositoryBase<Entities.CouponUserAss, int>, ICouponUserAssWriteRepository
    {
        public CouponUserAssWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}