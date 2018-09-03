using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Http
{
    public class HttpHelper
    {
        #region HttpClient
        private static string url = "http://10.1.1.1:8080/";
        //public string GetAll()
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri(url);

        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage response = client.GetAsync("api/goods/getlist?pageindex=0&pagesize=10").Result;  // Blocking call（阻塞调用）! 

        //    var result = "";
        //    if (response.IsSuccessStatusCode)
        //    {
        //        result = response.Content.ReadAsStringAsync().Result;
        //        //JavaScriptSerializer Serializer = new JavaScriptSerializer();
        //        //var items = Serializer.DeserializeObject(result);
        //        return result;
        //    }
        //    else
        //    {
        //        return result;
        //    }
        //}

        #region [ Demo ]
        #endregion
        #endregion

        #region HttpRequest
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <returns>响应JSON</returns>
        public static string HttpGet(string url)
        {
            string responseStr = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "Get";
                var response = request.GetResponse();
                Stream ReceiveStream = response.GetResponseStream();
                using (StreamReader stream = new StreamReader(ReceiveStream, Encoding.UTF8))
                {
                    responseStr = stream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseStr;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="postParameter">请求JSON</param>
        /// <returns>响应JSON</returns>
        public static string HttpPost(string url, string postParameter)
        {
            string responseStr = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/json";

                byte[] requestData = System.Text.Encoding.UTF8.GetBytes(postParameter);
                request.ContentLength = requestData.Length;

                Stream newStream = request.GetRequestStream();
                newStream.Write(requestData, 0, requestData.Length);
                newStream.Close();

                var response = request.GetResponse();
                Stream ReceiveStream = response.GetResponseStream();
                using (StreamReader stream = new StreamReader(ReceiveStream, Encoding.UTF8))
                {
                    responseStr = stream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseStr;
        }

        /// <summary>
        /// Http的 WebRequest的特定实现
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="JSONData"></param>
        /// <returns></returns>
        public static string HttpPostJson(string Url, string JSONData)
        {
            string strResult = string.Empty;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "application/json";
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);

                //声明一个HttpWebRequest请求  
                request.Timeout = 90000;
                //设置连接超时时间  
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.UTF8;

                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                streamReceive.Dispose();
                streamReader.Dispose();
            }
            catch (Exception ex) {
                throw ex;
            }
            return strResult;
        }

        #region [ Demo ]
        /// <summary>
        /// Get请求
        /// </summary>
        /// <returns>响应JSON</returns>
        public string GetAll()
        {
            string result = HttpGet(url + "api/goods/getlist?pageindex=0&pagesize=10");
            //JavaScriptSerializer Serializer = new JavaScriptSerializer();
            //var items = Serializer.DeserializeObject(result);
            return result;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="postParameter">请求JSON</param>
        /// <returns>响应JSON</returns>
        public string AddGood(string paramObj)
        {
            string result = HttpPost(url + "api/goods/add", paramObj);
            //JavaScriptSerializer Serializer = new JavaScriptSerializer();
            //var items = Serializer.DeserializeObject(result);
            return result;
        }
        #endregion
        #endregion
    }
}
