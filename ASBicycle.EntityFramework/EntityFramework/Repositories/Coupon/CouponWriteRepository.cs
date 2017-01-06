using Abp.EntityFramework;
using ASBicycle.Coupon;

namespace ASBicycle.EntityFramework.Repositories.Coupon
{
    public class CouponWriteRepository : ASBicycleRepositoryBase<Entities.Coupon, int>, ICouponWriteRepository
    {
        public CouponWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}