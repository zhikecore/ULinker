using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ULinker.BLL;
using ULinker.WebApp.Helpers;
using ULinker.Models.DO;
using ULinker.Models.VO;
using ULinker.Common.Utility;
using System.Collections;

namespace ULinker.WebApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //Data table
        public JsonResult GetDataTableRecords()
        {
            //Base Params
            DataTableParams dtParams = new DataTableParams(this);

            //Custom Parmas
            string keyword = Request["Keyword"];

            var dtRecords = ToDataTable(dtParams, keyword);

            return Json(dtRecords, JsonRequestBehavior.AllowGet);
        }

        public Object ToDataTable(DataTableParams dtParams, string keyword)
        {
            int total = 0;
            var filters = AdminUserService.Instance.GetBySomeWhere(keyword,dtParams.DisplayStart,dtParams.DisplayLength,out total);

            var aaData = new Object();
            if (filters.Count > dtParams.DisplayLength)
                aaData = filters.Skip(dtParams.DisplayStart).Take(dtParams.DisplayLength).Select(m => ToJson(m, dtParams));
            else
                aaData = filters.Select(m => ToJson(m, dtParams));

            return new { sEcho = dtParams.Echo, iTotalRecords = filters.Count, iTotalDisplayRecords = total, aaData = aaData };

        }


        //combox
        public JsonResult GetUsersForCombox()
        {
            return Json(AdminUserService.Instance.GetUsersForCombox(), JsonRequestBehavior.AllowGet);
        }

        // GET: Login
        public ActionResult Login()
        {
            var browserCookie = Request.Cookies[AccountHashKeys.AdminUserBrowserCookie];

            if (browserCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(browserCookie.Value);
                var email = ticket.Name;
                var password = ticket.UserData;
                var currentAdminUser = AdminUserService.Instance.GetByAccount(email);

                if (currentAdminUser == null)
                {
                    ClearClientCookie(AccountHashKeys.AdminUserBrowserCookie);
                    return View();
                }
                Session.Add(AccountHashKeys.CurrentAdminUser, AdminUserService.Instance.GetByAccount(email));
                return RedirectToAction("index", "default");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[AccountHashKeys.LoginModelStateErrorMessage] = true;
                return RedirectToAction("login", "user");
            }

            //ZebraService.AdminUser currentAdminUser = AdminUserService.Instance.GetByAccount(model.Account);
            AdminUser currentAdminUser = AdminUserService.Instance.GetByAccount(model.Account);


            if (currentAdminUser == null || String.IsNullOrEmpty(currentAdminUser.Account))
            {
                //todo:记录工具登录历史
                //AddLoginLog(currentAdminUser, "登陆", model.PhoneValideCode, false, "session失效");
                return RedirectToAction("login", "user");
            }
            /*
            //手机短信验证,验证码时效性验证
            //最近登录时间和当前时间差在一小时以内 有效
            if (!string.Equals(currentAdminUser.PhoneValideCode, model.PhoneValideCode))
            {
                //TODO:
                AddLoginLog(currentAdminUser, "登陆", model.PhoneValideCode, false, "短信验证码错误");
                TempData[AccountHashKeys.PhoneValideCodeErrorMessage] = true;
                return RedirectToAction("login", "account");
            }

            //20160825 修改有效时间30s
            if (!IsValideCodeExpired(currentAdminUser, "登陆", model.PhoneValideCode))
            {
                AddLoginLog(currentAdminUser, "登陆", model.PhoneValideCode, false, "当前短信验证码失效,请重新获取!");
                TempData[AccountHashKeys.PhoneValideCodeExpiredMessage] = true;
                return RedirectToAction("login", "account");
            }

            //记录登录结果信息
            AddLoginLog(currentAdminUser, "登陆", model.PhoneValideCode, true, "登陆成功");
            */


            // Session current valid AdminUser.
            Session.Add(AccountHashKeys.CurrentAdminUser, currentAdminUser);
            AdminUser currentUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
            return RedirectToAction("index", "default");
        }

        public ActionResult Logout()
        {
            Session[AccountHashKeys.CurrentAdminUser] = null;
            Session.Remove(AccountHashKeys.CurrentAdminUser);
            ClearClientCookie(AccountHashKeys.AdminUserBrowserCookie);

            return RedirectToAction("login", "user");
        }

        #region Private

        // Private methods
        private void ClearClientCookie(string cookieKey)
        {
            if (Request.Cookies[cookieKey] != null)
            {
                Response.Cookies.Add(new HttpCookie(cookieKey)
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }
        }

        private Object ToJson(AdminUser model, DataTableParams dtParams)
        {
            var json = model.AsJson() as Hashtable;
            json.Add("Actions", RenderViewHelper.RenderToString("_Actions", model, dtParams.Controller));

            return json;
        }

        #endregion
    }
}