using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Abp.Domain.Repositories;
using ASBicycle.Entities.Authen;
using ASBicycle.Web.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Abp.Domain.Uow;
using Abp.Json;
using Abp.UI;
using ASBicycle.Bike;
using ASBicycle.Bike.Dto;
using ASBicycle.Web.Extension.Fliter;
using ASBicycle.Web.Helper;
using ASBicycle.Web.Models.Authen;
using ASBicycle.Web.Models.Common;
using AutoMapper;
using LinqToDB.Common;

namespace ASBicycle.Web.Controllers
{
    public class HomeController : ASBicycleControllerBase
    {
        private readonly IRepository<BackUser> _backUserRepository;
        private readonly IRepository<Module> _moduleUserRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RoleModulePermission> _roleModulePermissionRepository;
        private readonly IRepository<Entities.School> _schoolAppService;

        public HomeController(IRepository<BackUser> backUserRepository, 
            IRepository<Module> moduleUserRepository, 
            IRepository<UserRole> userRoleRepository, 
            IRepository<RoleModulePermission> roleModulePermissionRepository,
            IRepository<Entities.School> schoolAppService)
        {
            _backUserRepository = backUserRepository;
            _moduleUserRepository = moduleUserRepository;
            _userRoleRepository = userRoleRepository;
            _roleModulePermissionRepository = roleModulePermissionRepository;
            _schoolAppService = schoolAppService;
        }
        //[AdminLayout]
        public ActionResult Index()
        {
            return RedirectToAction("List");
            //return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
        [AdminLayout]
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel();
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now.ToLocalTime();
                var despwd = DESProvider.EncryptString(model.Password);
                var tenant = string.IsNullOrEmpty(model.TenancyName) ? "default" : model.TenancyName.ToLower();
                var schoolid = 0;
                
                var school = _schoolAppService.GetAll().FirstOrDefault(t => t.TenancyName.ToLower() == tenant);

                if (school != null)
                {
                    schoolid = school.Id;
                }

                //var user =
                //    _backUserRepository.GetAll()
                //        .FirstOrDefault(
                //            u =>
                //                u.LoginName.ToLower() == model.UserNameOrEmail.ToLower() 
                //                && u.LoginPwd == despwd 
                //                && u.School_id == schoolid);
                var user =
                    _backUserRepository.GetAll()
                        .FirstOrDefault(
                            u =>
                                u.LoginName.ToLower() == model.UserNameOrEmail.ToLower()
                                && u.LoginPwd == despwd);
                if (user == null)
                    return Json(null);

                Mapper.CreateMap<BackUser, BackLoginModel>();

                var currentUser = Mapper.Map<BackLoginModel>(user);

                var roleIds = _userRoleRepository.GetAll().Where(ur => ur.UserId == currentUser.Id).Select(ur => ur.RoleId);

                var moduleIdList = _roleModulePermissionRepository.GetAll().Where(t => roleIds.Contains(t.RoleId))
                        .Select(t => t.ModuleId)
                        .Distinct()
                        .ToList();

                currentUser.Buttons.AddRange(_roleModulePermissionRepository.GetAll().Where(t => roleIds.Contains(t.RoleId) && (t.PermissionId != null || t.PermissionId > 0))
                    .Select(
                        t =>
                            new PermissionButtonModel
                            {
                                Action = t.Module == null ? "" : t.Module.Action,
                                Controller = t.Module == null ? "" : t.Module.Controller,
                                Code = t.Permission == null ? "" : t.Permission.Code,
                                Name = t.Permission == null ? "" : t.Permission.Name,
                                Icon = t.Permission == null ? "" : t.Permission.Icon
                            }));

                var moduleList = _moduleUserRepository.GetAll().ToList();
                //菜单列表
                SortMenuForTree(null, moduleIdList, moduleList, currentUser.Menus);

                Session["currentUser"] = currentUser;
                Session.Timeout = 720;  //12小时
                if (model.RememberMe)
                {
                    Session.Timeout = 43200;//30天
                }
                ////将用户名保存到票据中
                //var ticket = new FormsAuthenticationTicket(
                //    1,
                //    user.LoginName,
                //    now,
                //    //now.Add(_expirationTimeSpan),
                //    now.AddDays(365),
                //    model.RememberMe,
                //    //JsonSerializationHelper.SerializeWithType(currentUser),
                //    user.Id.ToString(),
                //    FormsAuthentication.FormsCookiePath
                //);
                ////加密
                //var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                //使用Cookie
                //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                //{
                //    HttpOnly = true,
                //    Secure = FormsAuthentication.RequireSSL,
                //    Path = FormsAuthentication.FormsCookiePath,

                //};
                //if (ticket.IsPersistent)
                //{
                //    cookie.Expires = ticket.Expiration;
                //}
                //if (FormsAuthentication.CookieDomain != null)
                //{
                //    cookie.Domain = FormsAuthentication.CookieDomain;
                //}
                //// 将加密后的票据保存到Cookie发送到客户端
                //HttpContext.Response.Cookies.Add(cookie);
            }
            if (!returnUrl.IsNullOrEmpty() && Url.IsLocalUrl(returnUrl))
                return Json(returnUrl);
            else
                return Json("List");
        }

        public ActionResult LoginOut()
        {
            //var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (cookie != null)
            //{
            //    cookie.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(cookie);
            //}
            //FormsAuthentication.SignOut();
            Session["currentUser"] = null;

            return RedirectToAction("Login");
        }

        /// <summary>
        /// 菜单节点
        /// </summary>
        /// <param name="parentId">父节点</param>
        /// <param name="moduleIdList"></param>
        /// <param name="allModule"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public void SortMenuForTree(int? parentId, List<int> moduleIdList, List<Module> allModule, List<SideBarMenuModel> model)
        {
            var modules = allModule.Where(m => m.ParentId == parentId && m.IsMenu && moduleIdList.Contains(m.Id)).OrderBy(m => m.OrderSort);
            foreach (var p in modules)
            {
                var menu = new SideBarMenuModel
                {
                    Name = p.Name,
                    Controller = p.Controller,
                    Action = p.Action,
                    Icon = p.Icon
                };
                SortMenuForTree(p.Id, moduleIdList, allModule, menu.ChildMenus);
                model.Add(menu);
            }
        }



        public ActionResult UploadTest()
        {   
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            //HttpPostedFileBase filedata = Request.Files["file"];
            //if (filedata != null)
            //{
            //    var bike = new BikeUploadInput {FileData = filedata};
            //    _bikeAppService.UploadBikePic(bike);
            //}
                
            return RedirectToAction("UploadTest");
        }
    }
}