using Abp.EntityFramework;
using ASBicycle.Coupon;

namespace ASBicycle.EntityFramework.Repositories.Coupon
{
    public class CouponPkgReadRepository : ReadonlyASBicycleRepositoryBase<Entities.CouponPackage, int>, ICouponPkgReadRepository
    {
        public CouponPkgReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}