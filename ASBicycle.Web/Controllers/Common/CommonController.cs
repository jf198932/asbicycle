using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Json;
using ASBicycle.Entities.Authen;
using ASBicycle.Web.Models.Common;

namespace ASBicycle.Web.Controllers.Common
{
    public class CommonController : ASBicycleControllerBase
    {
        private readonly IRepository<Module> _moduleUserRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RoleModulePermission> _roleModulePermissionRepository;

        public CommonController(IRepository<Module> moduleUserRepository, IRepository<UserRole> userRoleRepository, IRepository<RoleModulePermission> roleModulePermissionRepository)
        {
            _moduleUserRepository = moduleUserRepository;
            _userRoleRepository = userRoleRepository;
            _roleModulePermissionRepository = roleModulePermissionRepository;
        }

        // GET: Common
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly, UnitOfWork]
        public virtual ActionResult SidebarMenu()
        {
            var model = new List<SideBarMenuModel>();
            //var currentUser = Session["CurrentUser"] as BackUser;
            //var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            //if (cookie == null)
            //{
            //    return PartialView(model);
            //}

            //var ticket = FormsAuthentication.Decrypt(cookie.Value);

            //if (ticket == null)
            //{
            //    return PartialView(model);
            //}

            //var currentUser = JsonSerializationHelper.DeserializeWithType<BackLoginModel>(ticket.UserData);

            var currentUser = Session["currentUser"] as BackLoginModel;

            if (currentUser == null)
                return PartialView(model);
            
            return PartialView(currentUser.Menus);
        }

        
    }
}