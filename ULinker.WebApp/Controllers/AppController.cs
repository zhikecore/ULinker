using ULinker.BLL;
using ULinker.Common.Utility;
using ULinker.Models.DO;
using ULinker.WebApp.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ULinker.Models.VO;

namespace ULinker.WebApp.Controllers
{
    public class AppController : Controller
    {
        // GET: App
        public ActionResult Index()
        {
            return View();
        }

        #region Public

        //combox
        public JsonResult GetAppsForCombox()
        {
            return Json(AppService.Instance.GetAppsForCombox(), JsonRequestBehavior.AllowGet);
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
            var filters = AppService.Instance.GetBySomeWhere(keyword, dtParams.DisplayStart, dtParams.DisplayLength, out total);

            List<AppViewModel> _NewFilters = ToNewFilters(filters);

            var aaData = new Object();
            if (_NewFilters.Count > dtParams.DisplayLength)
                aaData = _NewFilters.Skip(dtParams.DisplayStart).Take(dtParams.DisplayLength).Select(m => ToJson(m, dtParams));
            else
                aaData = _NewFilters.Select(m => ToJson(m, dtParams));

            return new { sEcho = dtParams.Echo, iTotalRecords = _NewFilters.Count, iTotalDisplayRecords = total, aaData = aaData };

            //var aaData = new Object();
            //if (filters.Count > dtParams.DisplayLength)
            //    aaData = filters.Skip(dtParams.DisplayStart).Take(dtParams.DisplayLength).Select(m => ToJson(m, dtParams));
            //else
            //    aaData = filters.Select(m => ToJson(m, dtParams));

            //return new { sEcho = dtParams.Echo, iTotalRecords = filters.Count, iTotalDisplayRecords = total, aaData = aaData };
        }

        [HttpPost]
        public JsonResult Create(
            int appTypeId,
            int managerId,
            string name,
            string description
            )
        {

            var oResult = new Object();

            try
            {
                //登录验证
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
                if (currentAdminUser == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Url = "/admins/account/login"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                //必填参数验证
                if (String.IsNullOrEmpty(name))
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "应用名称为空!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                App app = new App
                {
                    AppTypeId=appTypeId,
                    ManagerId=managerId,
                    CreatorId=currentAdminUser.Id,
                    Name=name,
                    Description=description
                };
                
                bool bRet = AppService.Instance.Create(app);

                oResult = new
                {
                    Bresult = bRet,
                    Notice = bRet ? "操作成功!" : "操作失败!"
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("操作失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(
            int id,
            int appTypeId,
            int managerId,
            string name,
            string description)
        {
            var oResult = new Object();
            try
            {
                //登录验证
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
                if (currentAdminUser == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Url = "/admins/account/login"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                //必填参数验证
                if (String.IsNullOrEmpty(name))
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "应用名称为空!",
                    };

                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                App originApp = AppService.Instance.GetById(id);
                if (originApp == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "不存在该应用!请检查!"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                bool bRet = AppService.Instance.PhysicDelete(id,currentAdminUser.Account);

                oResult = new
                {
                    Bresult = bRet,
                    Notice = bRet ? "操作成功!" : "操作失败!"
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("操作失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public JsonResult PhysicDelete(int id)
        {
            var oResult = new Object();
            try
            {
                //登录验证
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
                if (currentAdminUser == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Url = "/admins/account/login"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                App originApp = AppService.Instance.GetById(id);
                if (originApp == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "不存在该应用!请检查!"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }
                
                bool bRet = AppService.Instance.PhysicDelete(id,currentAdminUser.Account);

                oResult = new
                {
                    Bresult = bRet,
                    Notice = bRet ? "操作成功!" : "操作失败!"
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("操作失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private

        private List<AppViewModel> ToNewFilters(List<App> filters)
        {
            List<AppViewModel> _NewFilters = new List<AppViewModel>();

            foreach (App item in filters)
            {
                if (item == null)
                    continue;

                AdminUser manager = AdminUserService.Instance.GetById(item.ManagerId);
                AdminUser creator = AdminUserService.Instance.GetById(item.CreatorId);

                _NewFilters.Add(new AppViewModel
                {
                    Id = item.Id,
                    CreatorId=item.CreatorId,
                    ManagerId=item.ManagerId,
                    Name = item.Name,
                    Description = item.Description,
                    CreateTime = item.CreateTime,
                    ModifyTime = item.ModifyTime,
                    Creator=creator.RealName,
                    Manager=manager.RealName
                });
            }

            return _NewFilters;
        }

        private Object ToJson(App model, DataTableParams dtParams)
        {
            var json = model.AsJson() as Hashtable;
            json.Add("Actions", RenderViewHelper.RenderToString("_Actions", model, dtParams.Controller));

            return json;
        }

        #endregion
    }
}