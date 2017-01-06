using Abp.EntityFramework;
using ASBicycle.Coupon;

namespace ASBicycle.EntityFramework.Repositories.Coupon
{
    public class CouponPkgAssWriteRepository : ASBicycleRepositoryBase<Entities.CouponPkgAss, int>, ICouponPkgAssWriteRepository
    {
        public CouponPkgAssWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}