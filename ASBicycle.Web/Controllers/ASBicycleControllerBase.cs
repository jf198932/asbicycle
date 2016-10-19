using System.Collections.Generic;
using System.Web.Mvc;
using Abp.Web.Mvc.Controllers;

namespace ASBicycle.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class ASBicycleControllerBase : AbpController
    {
        //private readonly IRepository<Module> _moduleRepository;
        //private readonly IRepository<UserRole> _userRoleRepository;
        //private readonly IRepository<RoleModulePermission> _roleModulePermissionRepository;
        //protected ASBicycleControllerBase(IRepository<Module> moduleRepository, IRepository<UserRole> userRoleRepository, IRepository<RoleModulePermission> roleModulePermissionRepository)
        //{
        //    LocalizationSourceName = ASBicycleConsts.LocalizationSourceName;
        //    _moduleRepository = moduleRepository;
        //    _userRoleRepository = userRoleRepository;
        //    _roleModulePermissionRepository = roleModulePermissionRepository;
        //    SetButton();
        //}

        protected ASBicycleControllerBase()
        {
            LocalizationSourceName = ASBicycleConsts.LocalizationSourceName;
            //SetButton();
        }

        /// <summary>
        /// Retuan DataTable Result
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iTotalRecords"></param>
        /// <param name="iTotalDisplayRecords"></param>
        /// <param name="aaData"></param>
        /// <returns></returns>
        protected ActionResult DataTableJsonResult(string sEcho, int iDisplayStart,
            int iTotalRecords, int iTotalDisplayRecords, IEnumerable<string[]> aaData)
        {
            return Json(new
            {
                sEcho = sEcho,
                iDisplayStart = iDisplayStart,
                iTotalRecords = iTotalRecords,
                iTotalDisplayRecords = iTotalDisplayRecords,
                aaData = aaData
            }, JsonRequestBehavior.AllowGet);
        }

        //public void SetButton()
        //{
        //    //var routeData = RouteData.Route.GetRouteData(this.HttpContext);
        //    //if (routeData != null)
        //    //{
        //    //    var controller = routeData.Values["controller"];
        //    //    var action = routeData.Values["action"];
        //    //}
        //    var controller = RouteData.Values["controller"].ToString().ToLower();
        //    //var action = RouteData.Values["action"].ToString().ToLower();
        //    var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //    var ticket = FormsAuthentication.Decrypt(cookie.Value);
        //    var currentUser = JsonSerializationHelper.DeserializeWithType<BackLoginModel>(ticket.UserData);

        //    var roleIds =
        //        _userRoleRepository.GetAll().Where(u => u.UserId == currentUser.Id).Select(u => u.RoleId).ToList();
        //    var module = _moduleRepository.FirstOrDefault(m => m.Controller.ToLower() == controller);

        //    if (module != null)
        //    {
        //        var permissionList =
        //            _roleModulePermissionRepository.GetAll()
        //                .Where(r => r.ModuleId == module.Id && roleIds.Contains(r.RoleId))
        //                .Select(r => r.Permission)
        //                .ToList();
        //        var buttonList =
        //            permissionList.Select(p => new PermissionButtonModel { Code = p.Code, Name = p.Name, Icon = p.Icon });
        //        ViewBag.ButtonList = buttonList;
        //    }
        //}
    }
}