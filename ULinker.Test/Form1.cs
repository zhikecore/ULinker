using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ULinker.Test.Common;
using ULinker.Test.ElasticSearch;
using ULinker.Test.Entity;

namespace ULinker.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                string appkey = "K97BiACFY291214Ci2PSHFOQPEMVBEB0";
                var tokenResult = WebApiHelper.GetSignToken(appkey);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void btnCompanylist_Click(object sender, EventArgs e)
        {
            /*int staffId = int.Parse(AppSettingsConfig.StaffId);
            var tokenResult = WebApiHelper.GetSignToken(staffId);
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("id", "1");
            parames.Add("name", "wahaha");
            Tuple<string, string> parameters = WebApiHelper.GetQueryString(parames);
            //var product1 = WebApiHelper.Get<ProductResultMsg>("http://localhost:14826/api/product/getproduct", parameters.Item1, parameters.Item2, staffId);
            var product1 = WebApiHelper.Get<ProductResultMsg>("http://47.104.128.14:8022/api/product/getproduct", parameters.Item1, parameters.Item2, staffId);
            //序列化
            string jsonData = JsonConvert.SerializeObject(product1);*/

            //1.获取token
            string appkey = @"K97BiACFY291214Ci2PSHFOQPEMVBEB0";
            //var tokenResult = WebApiHelper.GetSignToken(appkey);
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("companyname", "艾尔国际A12");
            Tuple<string, string> parameters = WebApiHelper.GetQueryString(parames);

            //2.GetListByCompanyName
            var companies = WebApiHelper.Get<HttpResponseMsg>("http://localhost:65152/api/Company/GetCompaniesByName", parameters.Item1, parameters.Item2, appkey);
        }

        private void btnGetOrders_Click(object sender, EventArgs e)
        {
            /*
             * int timetype,
            DateTime starttime,
            DateTime endtime,
            string companyname,
            string orderno,
            int productid,
            int paystate,
            int payment,
            string companyidstr,
            string saleIdStr,
            string exactcompany
             */

            string appkey = @"K97BiACFY291214Ci2PSHFOQPEMVBEB0";
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("timetype", "1");
            parames.Add("starttime", "2019-03-10 00:00:00");
            parames.Add("endtime", "2029-03-10 00:00:00");
            parames.Add("companyname", String.Empty);
            parames.Add("orderno", String.Empty);
            parames.Add("productid", "0");
            parames.Add("paystate", "0");//??
            parames.Add("payment", "0");//??
            parames.Add("companyidstr", String.Empty);
            parames.Add("saleIdStr", String.Empty);
            parames.Add("exactcompany", String.Empty);

            Tuple<string, string> parameters = WebApiHelper.GetQueryString(parames);

            var companies = WebApiHelper.Get<HttpResponseMsg>("http://localhost:65152/api/Order/GetOrders", parameters.Item1, parameters.Item2, appkey);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //            ElasticSearch.ESRequest es = new ESRequest("localhost", "people", "person");
            //            string json_query = @"
            //{ ""query"":{
            //        ""match"":{
            //            ""_id"":""1""
            //        }
            //    }
            //}
            //";
            //string strJsonResult = es.ExecuteQeury(json_query);
            string url = @"http://localhost:9200/people/person/1
{
  ""query"": {
    ""match_all"": { }
        }
    }";
            //string strJsonResult = ElasticSearch.ESRequest.GetHttpResponse(url,60);
            string res = ElasticSearch.ESRequest.GetHttpResponse(url, 6000);
            if (res != null)
            {
                string strJsonResult = ElasticSearch.ESRequest.GetHttpResponse(url, 60);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Property> properties = new List<Property>();
            properties.Add(new Property { PropId = 1, PropValue = "10" });
            properties.Add(new Property { PropId = 1, PropValue = "20" });
            properties.Add(new Property { PropId = 2, PropValue = "false" });
            properties.Add(new Property { PropId = 2, PropValue = "true" });

            //List<Property> ps = from c in properties
            //            group c by new { c.PropId } into g
            //            select new Property
            //            {
            //                PropId = g.Key.PropId,
            //                //PropValue = g.Sum(a => int.Parse(a.PropValue))
            //                PropValue = Deal(g.Key.PropId,g.ToList())
            //            };


            List<Property> s = properties.GroupBy(
                c => c.PropId,
                (key, group) => new Property
                {
                    PropId = key,
                    //items = group.ToList()
                    PropValue = Deal(key,group.ToList())
                }).ToList();

        }

        private string Deal(int propId, List<Property> ps)
        {
            string value = String.Empty;

            switch (propId)
            {
                case 1:
                    {
                        int _sum = 0;
                        foreach (Property p in ps)
                        {
                            if (p != null)
                            {
                                _sum += int.Parse(p.PropValue);
                            }
                        }
                        value = _sum.ToString();
                    }
                    break;
                case 2:
                    {
                        bool ret = true;
                        foreach (Property p in ps)
                        {
                            if (p != null)
                            {
                                ret &= Boolean.Parse(p.PropValue);
                            }
                        }
                        value = ret.ToString();
                    }
                    break;
            }

            return value;
        }

    }

    #region 

    public class Property
    {
        public int PropId { get; set; }
        public string PropValue { get; set; }
    }

    #endregion
}
