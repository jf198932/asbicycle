using Abp.Domain.Repositories;

namespace ASBicycle.Coupon
{
    public interface ICouponPkgReadRepository : IRepository<Entities.CouponPackage, int>
    {
         
    }
}