using Newtonsoft.Json;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ULinker.BLL;
using ULinker.Common.Utility;
using ULinker.Models.DO;
using ULinker.Services.Common;
using ULinker.Services.Models;

namespace ULinker.Services.Controllers
{
    public class ServiceController : ApiController
    {
        /// <summary>
        /// 根据用户名获取token
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appsecrect"></param>
        /// <returns></returns>
        public HttpResponseMessage GetToken(string appkey,string appsecrect)
        {
            lock (this)
            {
                //根据用户名获取token
                ResultMsg resultMsg = null;

                //判断参数是否合法
                if (String.IsNullOrEmpty(appkey))
                {
                    resultMsg = new ResultMsg();
                    resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                    resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                    resultMsg.Data = "";
                    Log4Helper.Error(this.GetType(), "ServiceController.GetToken失败。参数appkey为空!");
                    return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                }

                //检查是否存在
                UserPlatform userPlatform = UserPlatformService.Instance.GetByAppKey(appkey,appsecrect);
                string token = String.Empty;
                if (userPlatform == null)
                {
                    resultMsg = new ResultMsg();
                    resultMsg.StatusCode = (int)StatusCodeEnum.TokenInvalid;
                    resultMsg.Info = StatusCodeEnum.TokenInvalid.GetEnumText();
                    resultMsg.Data = "";
                    Log4Helper.Error(this.GetType(), String.Format("ServiceController.GetToken失败。请检查AppKey:{0}是否注册过!", appkey));
                    return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                }

                //token
                if (userPlatform != null && String.IsNullOrEmpty(userPlatform.Token))
                {
                    FeedBackResult result = RefreshToken(appkey);
                    if (!result.Result)
                    {
                        resultMsg = new ResultMsg();
                        resultMsg.StatusCode = (int)StatusCodeEnum.TokenInvalid;
                        resultMsg.Info = StatusCodeEnum.TokenInvalid.GetEnumText();
                        resultMsg.Data = "";
                        Log4Helper.Error(this.GetType(), String.Format("ServiceController.GetToken失败。刷新token:{0}失败!", token));
                        return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                    }
                }

                //重新获取一次
                userPlatform = UserPlatformService.Instance.GetByAppKey(appkey,appsecrect);

                //是否过期
                if (DateTime.Now > userPlatform.TokenExpireTime)
                {
                    //过期 重新生成
                    FeedBackResult result = RefreshToken(appkey);
                    if (!result.Result)
                    {
                        resultMsg = new ResultMsg();
                        resultMsg.StatusCode = (int)StatusCodeEnum.TokenInvalid;
                        resultMsg.Info = StatusCodeEnum.TokenInvalid.GetEnumText();
                        resultMsg.Data = "";
                        Log4Helper.Error(this.GetType(), String.Format("ServiceController.GetToken失败。刷新token:{0}失败!", token));
                        return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                    }
                }

                token = userPlatform.Token;

                //返回token信息
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                resultMsg.Info = "";
                resultMsg.Data = token;

                return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
            }
        }

        private FeedBackResult RefreshToken(string appkey)
        {
            FeedBackResult result = new FeedBackResult();
            try
            {
                //生成token并修改token过期时间
                string token = TextHelper.RandomSecretKey(32);
                DateTime tokenExpireTime = DateTime.Now.AddDays(1);//过期时间+一天

                //修改数据库
                bool bRet = UserPlatformService.Instance.UpdateToken(appkey, token, tokenExpireTime);
                if (bRet)
                {
                    result = new FeedBackResult
                    {
                        Result = true,
                        Message = "刷新Token成功"
                    };
                }
                else
                {
                    result = new FeedBackResult
                    {
                        Result = false,
                        Message = "刷新Token失败"
                    };
                }
            }
            catch (Exception ex)
            {
                result = new FeedBackResult
                {
                    Result = false,
                    Message = String.Format("刷新Token失败,异常:{0}", ex.Message)
                };
            }
            return result;
        }

    }
}
