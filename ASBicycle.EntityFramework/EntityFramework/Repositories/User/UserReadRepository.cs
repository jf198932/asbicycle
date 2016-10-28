using Abp.EntityFramework;
using ASBicycle.User;
using System.Linq;
using System.Threading.Tasks;

namespace ASBicycle.EntityFramework.Repositories.User
{
    public class UserReadRepository : ReadonlyASBicycleRepositoryBase<Entities.User, int>, IUserReadRepository
    {
        public UserReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}