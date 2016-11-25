using Abp.EntityFramework;
using ASBicycle.TbTroubleFeedback;

namespace ASBicycle.EntityFramework.Repositories.TbTroubleFeedback
{
    public class TbTroubleFeedbackReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Tb_trouble_feedback, int>, ITbTroubleFeedbackReadRepository
    {
        public TbTroubleFeedbackReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}