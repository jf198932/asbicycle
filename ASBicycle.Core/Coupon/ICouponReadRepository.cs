using Abp.Domain.Repositories;

namespace ASBicycle.Coupon
{
    public interface ICouponReadRepository : IRepository<Entities.Coupon, int>
    {
         
    }
}