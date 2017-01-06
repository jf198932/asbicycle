using Abp.EntityFramework;
using ASBicycle.Coupon;

namespace ASBicycle.EntityFramework.Repositories.Coupon
{
    public class CouponUserAssReadRepository : ReadonlyASBicycleRepositoryBase<Entities.CouponUserAss, int>, ICouponUserAssReadRepository
    {
        public CouponUserAssReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}