using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ULinker.Test.ElasticSearch
{
    public class ESRequest
    {
        string es_host;
        string es_port;
        string es_index;
        string es_type;
        private string url;

        public ESRequest(string host, string index, string type, string port = "9200")
        {
            es_host = host;
            es_port = port;
            es_index = index;
            es_type = type;

            string requst_cache = "request_cache=true";
            url = string.Format("http://{0}:{1}/{2}/{3}/_search?{4}", es_host, es_port, es_index, es_type, requst_cache);
        }

        public string ExecuteQeury(string json_query)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "aplication/json";
            request.Method = "GET";
            request.Timeout = 1000 * 60;
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(json_query);
                sw.Flush();
                sw.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        ///
        /// Get请求
        /// 
        /// 
        /// 字符串
        public static string GetHttpResponse(string url, int Timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }
}
