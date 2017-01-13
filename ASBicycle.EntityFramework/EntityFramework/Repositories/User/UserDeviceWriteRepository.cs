using Abp.EntityFramework;
using ASBicycle.User;

namespace ASBicycle.EntityFramework.Repositories.User
{
    public class UserDeviceWriteRepository : ASBicycleRepositoryBase<Entities.UserDevice, int>, IUserDeviceWriteRepository
    {
        public UserDeviceWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}