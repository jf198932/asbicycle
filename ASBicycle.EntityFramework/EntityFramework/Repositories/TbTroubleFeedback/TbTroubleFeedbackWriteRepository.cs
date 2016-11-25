using Abp.EntityFramework;
using ASBicycle.TbTroubleFeedback;

namespace ASBicycle.EntityFramework.Repositories.TbTroubleFeedback
{
    public class TbTroubleFeedbackWriteRepository : ASBicycleRepositoryBase<Entities.Tb_trouble_feedback, int>, ITbTroubleFeedbackWriteRepository
    {
        public TbTroubleFeedbackWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}