using Elasticsearch.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ULinker.Common.Utility;
using ULinker.Models.DO;
using ULinker.Models.VO;

namespace ULinker.WebApp.Services
{
    public class PersonService
    {
        private static PersonService _Instance = null;

        public static PersonService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new PersonService();
                return _Instance;
            }
        }

        private PersonService()
        {

        }

        /// 
        /// </summary>
        /// <returns></returns>
        public Notice Add()
        {
            Notice notice = new Notice();
            try
            {
                var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
    .RequestTimeout(TimeSpan.FromMinutes(2));

                var lowlevelClient = new ElasticLowLevelClient(settings);

                /*var people = new object[]
                {
                new { index = new { _index = "people", _type = "person", _id = "1"  }},
                new { FirstName = "Martijn", LastName = "Laarman" },
                new { index = new { _index = "people", _type = "person", _id = "2"  }},
                new { FirstName = "Greg", LastName = "Marzouka" },
                new { index = new { _index = "people", _type = "person", _id = "3"  }},
                new { FirstName = "Russ", LastName = "Cam" },
                };

                var indexResponse = lowlevelClient.Bulk<Stream>(people);
                Stream responseStream = indexResponse.Body;*/

                var person = new Models.Person
                {
                    FirstName = "Martijn",
                    LastName = "Laarman"
                };
                var indexResponse = lowlevelClient.Index<byte[]>("people", "person", "1", person);
                byte[] responseBytes = indexResponse.Body;

                notice = new Notice
                {
                    Result = true,
                    Message = "创建成功!"
                };
            }
            catch (ElasticsearchClientException ex)
            {
                notice = new Notice
                {
                    Result = false,
                    Message = ex.Message
                };
            }
            return notice;
        }

        public Notice Add(AdminUser user)
        {
            Notice notice = new Notice();
            try
            {
                var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
    .RequestTimeout(TimeSpan.FromMinutes(2));

                var lowlevelClient = new ElasticLowLevelClient(settings);

                /*var people = new object[]
                {
                new { index = new { _index = "people", _type = "person", _id = "1"  }},
                new { FirstName = "Martijn", LastName = "Laarman" },
                new { index = new { _index = "people", _type = "person", _id = "2"  }},
                new { FirstName = "Greg", LastName = "Marzouka" },
                new { index = new { _index = "people", _type = "person", _id = "3"  }},
                new { FirstName = "Russ", LastName = "Cam" },
                };

                var indexResponse = lowlevelClient.Bulk<Stream>(people);
                Stream responseStream = indexResponse.Body;*/

                //var person = new Models.Person
                //{
                //    FirstName = "Martijn",
                //    LastName = "Laarman"
                //};
                //lowlevelClient.de
                var indexResponse = lowlevelClient.Index<byte[]>("user", "guest", user.Id.ToString(), user);
                byte[] responseBytes = indexResponse.Body;

                notice = new Notice
                {
                    Result = true,
                    Message = "创建成功!"
                };
            }
            catch (ElasticsearchClientException ex)
            {
                notice = new Notice
                {
                    Result = false,
                    Message = ex.Message
                };
            }
            return notice;
        }

        public Notice Search()
        {
            Notice notice = new Notice();
            try
            {
                var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
   .RequestTimeout(TimeSpan.FromMinutes(2));

                var lowlevelClient = new ElasticLowLevelClient(settings);
                /*var searchResponse = lowlevelClient.Search<byte[]>("people", "person", @"
{
    ""from"": 0,
    ""size"": 10,
    ""query"": {
        ""match"": {
            ""firstName"": ""Martijn""
        }
    }
}");

                var responseBytes = searchResponse.Body;
                string s = bytetohex(responseBytes);*/

                //根据字段值匹配
                /*var searchResponse = lowlevelClient.Search<string>("user", "guest", new
                {
                    from = 0,
                    size = 10,
                    query = new
                    {
                        match = new
                        {
                            FirstName="Martijn"
                        }
                    }
                });*/
                //根据不同的参数 来构建不同的查询条件

                var searchResponse = lowlevelClient.Search<string>("user", "guest", new
                {
                    from = 0,
                    size = 10,
                    query = new
                    {
                        match_all = new
                        {

                        }
                    },
                    sort = new
                    {
                        Id = new
                        {
                            order = "desc"
                        }
                    }
                });

                bool successful = searchResponse.Success;
                var responseJson = searchResponse.Body;

                //此处模拟在不建实体类的情况下，反转将json返回回dynamic对象
                //var DynamicObject = JsonConvert.DeserializeObject<dynamic>(json);
                //List<ESearchRoot> roots = JsonHelper.JSONStringToList<ESearchRoot>(responseJson);
                /*ESearchRoot root = JsonHelper.JSONStringObject<ESearchRoot>(responseJson);
                int _total = 0;
                if (root != null)
                {
                    _total = root.hits.total;
                    //foreach(HitsItem item in root.hits.hits)
                    //{
                    //    item._source.
                    //}
                }

                if (!successful)
                {
                    return new Notice
                    {
                        Result=successful,
                        Message=searchResponse.ServerError.Error.Reason
                    };
                }

                notice = new Notice
                {
                    Result=true,
                    Message=String.Empty
                };*/

            }
            catch (ElasticsearchClientException ex)
            {
                notice = new Notice
                {
                    Result = false,
                    Message = ex.Message
                };
            }
            return notice;
        }

        public string bytetohex(byte[] byteArray)//byte[]转HEXString
        {
            // string str = "";
            var str = new System.Text.StringBuilder();
            int start = Environment.TickCount;//计时器
            Console.WriteLine("wait...");
            for (int i = 0; i < byteArray.Length; i++)
            {
                // Application.DoEvents();//界面不卡死
                // str += String.Format("{0:X} ", byteArray[i]);//普通拼接
                str.Append(String.Format("{0:X} ", byteArray[i]));//var拼接
            }
            string s = str.ToString();
            Console.WriteLine(Math.Abs(Environment.TickCount - start));//在调试窗口可查用时
            return s;
        }
    }
}