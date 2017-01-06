using Abp.Domain.Repositories;

namespace ASBicycle.Coupon
{
    public interface ICouponPkgWriteRepository : IRepository<Entities.CouponPackage, int>
    {
         
    }
}