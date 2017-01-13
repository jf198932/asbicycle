using Abp.EntityFramework;
using ASBicycle.User;

namespace ASBicycle.EntityFramework.Repositories.User
{
    public class UserDeviceReadRepository : ReadonlyASBicycleRepositoryBase<Entities.UserDevice, int>, IUserDeviceReadRepository
    {
        public UserDeviceReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}