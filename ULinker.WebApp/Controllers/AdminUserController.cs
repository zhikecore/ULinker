using Elasticsearch.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ULinker.Common.Utility;
using ULinker.Models;
using ULinker.Models.DO;
using ULinker.Models.VO;
using ULinker.WebApp.Helpers;

namespace ULinker.WebApp.Controllers
{
    public class AdminUserController : Controller
    {
        // GET: AdminUser
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
            //var filters = AdminUserService.Instance.GetBySomeWhere(keyword, dtParams.DisplayStart, dtParams.DisplayLength, out total);
            var filters = GetBySomeWhere(keyword, dtParams.DisplayStart, dtParams.DisplayLength, out total);

            var aaData = new Object();
            if (filters.Count > dtParams.DisplayLength)
                aaData = filters.Skip(dtParams.DisplayStart).Take(dtParams.DisplayLength).Select(m => ToJson(m, dtParams));
            else
                aaData = filters.Select(m => ToJson(m, dtParams));

            return new { sEcho = dtParams.Echo, iTotalRecords = filters.Count, iTotalDisplayRecords = total, aaData = aaData };

        }

        public List<AdminUser> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<AdminUser> users = new List<AdminUser>();

            total = 0;
            try
            {
                var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
   .RequestTimeout(TimeSpan.FromMinutes(2));

                var lowlevelClient = new ElasticLowLevelClient(settings);

                //根据不同的参数 来构建不同的查询条件
                var request = new object();
                if (!String.IsNullOrEmpty(keyword))
                {
                    request = new
                    {
                        from = limit,
                        size = pageSize,
                        query = new
                        {
                            match = new
                            {
                                Description = keyword
                            }
                        },
                        highlight = new
                        {
                            fields = new
                            {
                                Description = new { }
                            }
                        },
                        sort = new
                        {
                            Id = new
                            {
                                order = "desc"
                            }
                        }
                    };
                }
                else
                {
                    request = new
                    {
                        from = limit,
                        size = pageSize,
                        query = new
                        {
                            match_all = new
                            {

                            }
                        },
                        highlight = new
                        {
                            fields = new
                            {
                                Description = new { }
                            }
                        },
                        sort = new
                        {
                            Id = new
                            {
                                order = "desc"
                            }
                        }
                    };
                }


                var searchResponse = lowlevelClient.Search<string>("user", "guest", request);

                bool successful = searchResponse.Success;
                var responseJson = searchResponse.Body;

                if (!successful)
                {
                    return users;
                }

                ESearchRoot<User> root = JsonHelper.JSONStringObject<ESearchRoot<User>>(responseJson);
                if (root != null)
                {
                    total = root.hits.total;
                    foreach (HitsItem<User> item in root.hits.hits)
                    {
                        if (item._source != null)
                        {
                            string highlightDescription = String.Empty;
                            StringBuilder sbDs = new StringBuilder();
                            if (item.highlight != null && item.highlight.Description.Count > 0)
                            {
                                //ighlightDescription = item.highlight.Description[0];
                                foreach (var d in item.highlight.Description)
                                {
                                    sbDs.Append(d);
                                }
                                highlightDescription = sbDs.ToString();
                            }

                            AdminUser user = new AdminUser
                            {
                                Id = item._source.Id,
                                RealName = item._source.RealName,
                                Account = item._source.Account,
                                Email = item._source.Email,
                                Phone = item._source.Phone,
                                //IsUse=item._source.IsUse,
                                Avatar = item._source.Avatar,
                                Description = item._source.Description,
                                HighlightDescription = highlightDescription,
                                CreateTime = item._source.CreateTime,
                                ModifyTime = item._source.ModifyTime
                            };
                            users.Add(user);
                        }
                    }
                }

                return users;
            }
            catch (ElasticsearchClientException ex)
            {
                //Log4Helper.Error
            }
            return users;
        }

        public JsonResult GetById(int id)
        {
            AdminUser user = _GetById(id);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(int id, string realname, string phone, string description)
        {
            var oResult = new Object();
            try
            {
                AdminUser user = new AdminUser
                {
                    Id = id,
                    Account = realname,
                    RealName = realname,
                    Phone = phone,
                    Description = description,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now
                };

                var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
    .RequestTimeout(TimeSpan.FromMinutes(2));

                var lowlevelClient = new ElasticLowLevelClient(settings);
                var indexResponse = lowlevelClient.Index<byte[]>("user", "guest", user.Id.ToString(), user);
                byte[] responseBytes = indexResponse.Body;

                oResult = new
                {
                    Result = true,
                    Message = "创建成功!"
                };
            }
            catch (ElasticsearchClientException ex)
            {
                oResult = new
                {
                    Result = true,
                    Message = "创建失败!"
                };
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(int id, string realname, string description)
        {
            var oResult = new Object();
            try
            {
                var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
   .RequestTimeout(TimeSpan.FromMinutes(2));

                var lowlevelClient = new ElasticLowLevelClient(settings);

                var searchResponse = lowlevelClient.Update<string>("user", "guest", id.ToString(), new
                {
                    doc = new
                    {
                        RealName = realname,
                        Description = description
                    }
                });

                bool successful = searchResponse.Success;

                oResult = new
                {
                    Bresult = successful,
                    Notice = successful ? "修改成功!" : "修改失败!"
                };

                var responseJson = searchResponse.Body;

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("修改失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var oResult = new Object();
            try
            {
                var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
   .RequestTimeout(TimeSpan.FromMinutes(2));

                var lowlevelClient = new ElasticLowLevelClient(settings);

                var searchResponse = lowlevelClient.Delete<string>("user", "guest", id.ToString());

                bool successful = searchResponse.Success;

                oResult = new
                {
                    Bresult = successful,
                    Notice = successful ? "删除成功!" : "删除失败!"
                };

                var responseJson = searchResponse.Body;

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("删除失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        public AdminUser _GetById(int id)
        {
            AdminUser user = null;

            try
            {
                var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
   .RequestTimeout(TimeSpan.FromMinutes(2));

                var lowlevelClient = new ElasticLowLevelClient(settings);
                var searchResponse = lowlevelClient.Search<string>("user", "guest", new
                {
                    query = new
                    {
                        match = new
                        {
                            Id = id
                        }
                    }
                });

                bool successful = searchResponse.Success;

                if (!successful)
                {
                    return user;
                }

                var responseJson = searchResponse.Body;

                ESearchRoot<User> root = JsonHelper.JSONStringObject<ESearchRoot<User>>(responseJson);

                HitsItem<User> item = root.hits.hits[0];
                if (item != null && item._source != null)
                {
                    user = new AdminUser
                    {
                        Id = item._source.Id,
                        RealName = item._source.RealName,
                        Account = item._source.Account,
                        Email = item._source.Email,
                        Phone = item._source.Phone,
                        //IsUse=item._source.IsUse,
                        Avatar = item._source.Avatar,
                        Description = item._source.Description,
                        CreateTime = item._source.CreateTime,
                        ModifyTime = item._source.ModifyTime
                    };
                }
            }
            catch (Exception ex)
            {

            }
            return user;
        }

        private Object ToJson(AdminUser model, DataTableParams dtParams)
        {
            var json = model.AsJson() as Hashtable;
            json.Add("Actions", RenderViewHelper.RenderToString("_Actions", model, dtParams.Controller));

            return json;
        }
    }
}