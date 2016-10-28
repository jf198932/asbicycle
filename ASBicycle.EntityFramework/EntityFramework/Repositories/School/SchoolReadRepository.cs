using Abp.EntityFramework;
using ASBicycle.School;

namespace ASBicycle.EntityFramework.Repositories.School
{
    public class SchoolReadRepository : ReadonlyASBicycleRepositoryBase<Entities.School, int>, ISchoolReadRepository
    {
        public SchoolReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}