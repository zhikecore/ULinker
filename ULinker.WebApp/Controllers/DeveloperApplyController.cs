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
    public class DeveloperApplyController : BaseController
    {
        // GET: DeveloperApply
        public ActionResult Index()
        {
            return View();
        }

        #region Public

        //Data table
        public JsonResult GetDataTableRecords()
        {
            //Base Params
            DataTableParams dtParams = new DataTableParams(this);

            //Custom Parmas
            //string keyword = Request["Keyword"];
            int appId = null == Request["appId"] ? 0 : int.Parse(Request["appId"]);
            int developerId = null == Request["developerId"] ? 0 : int.Parse(Request["developerId"]);
            int platformId = null == Request["platformId"] ? 0 : int.Parse(Request["platformId"]);

            var dtRecords = ToDataTable(dtParams,appId,platformId);

            return Json(dtRecords, JsonRequestBehavior.AllowGet);
        }

        public Object ToDataTable(DataTableParams dtParams, int appId, int platformId)
        {
            int total = 0;
            var filters = UserPlatformService.Instance.GetBySomeWhere(appId,platformId,dtParams.DisplayStart, dtParams.DisplayLength, out total);

            List<UserPlatformViewModel> _NewFilters = ToNewFilters(filters);

            var aaData = new Object();
            if (_NewFilters.Count > dtParams.DisplayLength)
                aaData = _NewFilters.Skip(dtParams.DisplayStart).Take(dtParams.DisplayLength).Select(m => ToJson(m, dtParams));
            else
                aaData = _NewFilters.Select(m => ToJson(m, dtParams));

            return new { sEcho = dtParams.Echo, iTotalRecords = _NewFilters.Count, iTotalDisplayRecords = total, aaData = aaData };
        }

        [HttpPost]
        public JsonResult Create(
            int platformId,
            int appId,
            string appKey,
            string appSecrect
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
                if (platformId == 0)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "请选择平台!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                if (appId == 0)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "请选择应用!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                if (String.IsNullOrEmpty(appKey) || String.IsNullOrEmpty(appSecrect))
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "请填写AppKey和AppSecrect!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                UserPlatform up = new UserPlatform
                {
                    CreatorId = currentAdminUser.Id,
                    PlatformId=platformId,
                    AppId=appId,
                    AppKey=appKey,
                    AppSecrect=appSecrect
                };

                //20180315
                bool isExist = UserPlatformService.Instance.IsExist(platformId,appId);
                if (isExist)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "该应用已经在该平台申请过!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                bool bRet =UserPlatformService .Instance.Create(up);

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
            int platformId,
            int appId,
            string appKey,
            string appSecrect)
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
                if (platformId == 0)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "请选择平台!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                if (appId == 0)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "请选择应用!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                if (String.IsNullOrEmpty(appKey) || String.IsNullOrEmpty(appSecrect))
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "请填写AppKey和AppSecrect!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                //App originApp = AppService.Instance.GetById(id);
                UserPlatform originUp = UserPlatformService.Instance.GetById(id);
                if (originUp == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "不存在!请检查!"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                originUp.PlatformId = platformId;
                originUp.AppId = appId;
                originUp.AppKey = appKey;
                originUp.AppSecrect = appSecrect;

                //20180315
                bool isExist = UserPlatformService.Instance.IsExist(platformId, appId);
                if (isExist)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "该应用已经在该平台申请过!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                bool bRet = UserPlatformService.Instance.Update(originUp);

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

                bool bRet = AppService.Instance.PhysicDelete(id, currentAdminUser.Account);

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
        public JsonResult BuildAppKey()
        {
            var oResult = new Object();

            string appkey = String.Empty;

            appkey = TextHelper.RandomSecretKey(32);
            bool isExist = UserPlatformService.Instance.IsExistAppKey(appkey);

            if (!isExist)
            {
                oResult = new
                {
                    Bresult = true,
                    Notice = appkey,
                };
            }
            else
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = "AppKey已经存在,请重新生成!",
                };
            }

            return Json(oResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BuildAppSecrect()
        {
            var oResult = new Object();
            oResult = new
            {
                Bresult = true,
                Notice = Guid.NewGuid().ToString(),
            };
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private

        private List<UserPlatformViewModel> ToNewFilters(List<UserPlatform> filters)
        {
            List<UserPlatformViewModel> _NewFilters = new List<UserPlatformViewModel>();

            foreach (UserPlatform item in filters)
            {
                if (item == null)
                    continue;

                AdminUser developer = AdminUserService.Instance.GetById(item.CreatorId);
                App app = AppService.Instance.GetById(item.AppId);
                Platform platform = PlatformService.Instance.GetById(item.PlatformId);

                _NewFilters.Add(new UserPlatformViewModel
                {
                    Id = item.Id,
                    CreatorId=item.CreatorId,
                    PlatformId=item.PlatformId,
                    AppId=item.AppId,
                    AppKey=item.AppKey,
                    AppSecrect=item.AppSecrect,
                    Token=item.Token,
                    Description = item.Description,
                    TokenExpireTime=item.TokenExpireTime,
                    CreateTime = item.CreateTime,
                    ModifyTime = item.ModifyTime,
                    Platform=platform.Name,
                    App=app.Name
                });
            }

            return _NewFilters;
        }

        private Object ToJson(UserPlatform model, DataTableParams dtParams)
        {
            var json = model.AsJson() as Hashtable;
            json.Add("Actions", RenderViewHelper.RenderToString("_Actions", model, dtParams.Controller));

            return json;
        }

        #endregion
    }
}