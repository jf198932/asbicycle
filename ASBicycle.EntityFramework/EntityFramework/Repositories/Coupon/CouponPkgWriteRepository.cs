using Abp.EntityFramework;
using ASBicycle.Coupon;

namespace ASBicycle.EntityFramework.Repositories.Coupon
{
    public class CouponPkgWriteRepository : ASBicycleRepositoryBase<Entities.CouponPackage, int>, ICouponPkgWriteRepository
    {
        public CouponPkgWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}