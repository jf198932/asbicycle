using Abp.EntityFramework;
using ASBicycle.School;

namespace ASBicycle.EntityFramework.Repositories.School
{
    public class SchoolWriteRepository : ASBicycleRepositoryBase<Entities.School, int>, ISchoolWriteRepository
    {
        public SchoolWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}