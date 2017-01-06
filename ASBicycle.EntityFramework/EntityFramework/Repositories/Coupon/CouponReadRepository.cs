using Abp.EntityFramework;
using ASBicycle.Coupon;

namespace ASBicycle.EntityFramework.Repositories.Coupon
{
    public class CouponReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Coupon, int>, ICouponReadRepository
    {
        public CouponReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}