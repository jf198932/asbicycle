using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Bike.Dto;
using ASBicycle.User.Dto;

namespace ASBicycle.User
{
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 登录验证token
        /// </summary>
        /// <param name="checkIdentityInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task<UserOutput> CheckIdentity(CheckIdentityInput checkIdentityInput);
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phoneNumInput"></param>
        /// <returns></returns>
        [HttpGet]
        Task<CheckCodeOutput> GetCheckCode([FromUri]PhoneNumInput phoneNumInput);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task UpdateUser(UserInput userInput);
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userIdInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task LoginOut(UserIdInput userIdInput);
        /// <summary>
        /// 绑定锁
        /// </summary>
        /// <param name="userBikeInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task<BikeOutput> BindBike(UserBikeInput userBikeInput);
        /// <summary>
        /// 解绑锁
        /// </summary>
        /// <param name="userBikeInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task DeBindBike(UserBikeInput userBikeInput);
        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="userBikeInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task LockBike(UserBikeInput userBikeInput);
        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="userBikeInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task OpenBike(UserBikeInput userBikeInput);
        /// <summary>
        /// 修复异常状态
        /// </summary>
        /// <param name="userBikeInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task RepairBike(UserBikeInput userBikeInput);

        /// <summary>
        /// 异常转报警
        /// </summary>
        /// <param name="userBikeInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task AlarmBike(UserBikeInput userBikeInput);
        [HttpGet]
        Task<UserBikeOutput> GetUserBike([FromUri]UserIdInput userIdInput);
        [HttpPost]
        UserUploadOutput UploadUserPic();

        [HttpPost]
        Task<UserOutput> UserLogin(CheckLoginInput checkLoginInput);
        [HttpPost]
        Task<UserOutput> UserRegister(RegisterUserInput modelIntput);

        [HttpGet]
        MianzeOutput Mianze();
        [HttpGet]
        MianzeOutput About();
        [HttpGet]
        Task<UserOutput> GetUserInfo([FromUri] PhoneNumInput phoneNumInput);

        [HttpGet]
        Task<CheckCodeOutput> GetCheckCodeRegist([FromUri]PhoneNumInput phoneNumInput);

        [HttpPost]
        Task<UserUploadOutput> UploadUserHeadPic();

        [HttpPost]
        Task<UserOutput> UpdateUserNickName(UserInput userInput);

        [HttpGet]
        Task<CertificationOutput> GetUserCertificationStatus([FromUri] UserIdInput userIdInput);
    }
}