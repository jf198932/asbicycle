using Abp.EntityFramework;
using ASBicycle.Coupon;

namespace ASBicycle.EntityFramework.Repositories.Coupon
{
    public class CouponPkgAssReadRepository : ReadonlyASBicycleRepositoryBase<Entities.CouponPkgAss, int>, ICouponPkgAssReadRepository
    {
        public CouponPkgAssReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}