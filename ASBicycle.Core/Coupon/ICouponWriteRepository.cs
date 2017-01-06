using Abp.Domain.Repositories;

namespace ASBicycle.Coupon
{
    public interface ICouponWriteRepository : IRepository<Entities.Coupon, int>
    {
         
    }
}