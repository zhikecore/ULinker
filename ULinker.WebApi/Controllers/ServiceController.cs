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
using ULinker.WebApi.Common;
using ULinker.WebApi.Models;

namespace ULinker.WebApi.Controllers
{
    public class ServiceController : ApiController
    {
        /// <summary>
        /// 根据用户名获取token
        /// </summary>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public HttpResponseMessage GetToken(string appkey,string appsecrect)
        {
            lock (this)
            {
                //增加对appkey+secrect的解密

                Log4Helper.Info(this.GetType(),String.Format("进入ServiceController.GetToken。appkey:{0},appsecrect:{1}",appkey,appsecrect));

                //根据用户名获取token
                ResultMsg resultMsg = null;

                //判断参数是否合法
                if (String.IsNullOrEmpty(appkey) || String.IsNullOrEmpty(appsecrect))
                {
                    resultMsg = new ResultMsg();
                    resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                    resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                    resultMsg.Data = "";
                    Log4Helper.Error(this.GetType(), "ServiceController.GetToken失败。请检查参数appkey或appsecrect为空!");
                    return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                }

                //1.检查缓存中是否有,有就结束
                Token token = (Token)HttpRuntime.Cache.Get(appkey);
                if (token == null)
                {
                    //1.1 从数据库取
                    UserPlatform userPlatform = UserPlatformService.Instance.GetByAppKey(appkey,appsecrect);
                    if (userPlatform == null)
                    {
                        resultMsg = new ResultMsg();
                        resultMsg.StatusCode = (int)StatusCodeEnum.TokenInvalid;
                        resultMsg.Info = StatusCodeEnum.TokenInvalid.GetEnumText();
                        resultMsg.Data = "";
                        Log4Helper.Error(this.GetType(), String.Format("ServiceController.GetToken失败。请检查AppKey:{0}是否注册过!", appkey));
                        return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                    }

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

                        //重新获取一次
                        userPlatform = UserPlatformService.Instance.GetByAppKey(appkey,appsecrect);
                    }

                    //是否过期
                    if (DateTime.Now > userPlatform.TokenExpireTime)
                    {
                        //过期 刷新
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
                        //获取
                        userPlatform = UserPlatformService.Instance.GetByAppKey(appkey,appsecrect);
                    }

                    token = new Token
                    {
                        AppKey = userPlatform.AppKey,
                        SignToken = userPlatform.Token,
                        ExpireTime = userPlatform.TokenExpireTime
                    };

                    //插入缓存
                    HttpRuntime.Cache.Insert(token.AppKey, token, null, token.ExpireTime, TimeSpan.Zero);
                }
                else if (token.ExpireTime < DateTime.Now)
                {
                    //过期 刷新
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

                    //获取
                    UserPlatform userPlatform = UserPlatformService.Instance.GetByAppKey(appkey,appsecrect);

                    token = new Token
                    {
                        AppKey = userPlatform.AppKey,
                        SignToken = userPlatform.Token,
                        ExpireTime = userPlatform.TokenExpireTime
                    };

                    //移除缓存
                    HttpRuntime.Cache.Remove(token.AppKey);

                    //插入缓存
                    HttpRuntime.Cache.Insert(token.AppKey, token, null, token.ExpireTime, TimeSpan.Zero);
                }

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
