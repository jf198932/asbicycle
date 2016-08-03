using Abp.EntityFramework;
using ASBicycle.User;
using System.Linq;
using System.Threading.Tasks;

namespace ASBicycle.EntityFramework.Repositories.User
{
    public class UserRepository : ASBicycleRepositoryBase<Entities.User, int>, IUserRepository
    {
        public UserRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        //public Task<bool> CheckIdentity(string phoneNum, string token)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public async Task<bool> CheckLogin(string phoneNum, string checkCode)
        //{
        //    //todo 验证码现在写死，要用随机生成的，先根据手机号码获取存放内存中的验证码
        //    var result = await GetAllListAsync(u=>u.Phone == phoneNum && checkCode == "1234");
        //    return result != null;
        //}

        //public async Task<bool> RegisterUser(string phoneNum)
        //{
        //    var result = await InsertAsync(new Entities.User {Phone = phoneNum});
        //    return result != null;
        //}
    }
}