using Abp.EntityFramework;
using ASBicycle.User;
using System.Linq;
using System.Threading.Tasks;

namespace ASBicycle.EntityFramework.Repositories.User
{
    public class UserWriteRepository : ASBicycleRepositoryBase<Entities.User, int>, IUserWriteRepository
    {
        public UserWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}