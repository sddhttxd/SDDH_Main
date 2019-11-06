using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Demo
{
    public class CdiscountDemo
    {
        public static string UpdateToken()
        {
            var tokenUrl = "https://sts.cdiscount.com/users/httpIssue.svc/?realm=https://wsvc.cdiscount.com/MarketplaceAPIService.svc";
            HttpWebRequest request = WebRequest.Create(tokenUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "GET";
            string base64Credentials = GetEncodedCredentials();
            request.Headers.Add("Authorization", "Basic " + base64Credentials);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            string result = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(result))
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(result);
                result = xmlDoc.InnerText;
            }
            else
            {
                Console.WriteLine("Token获取异常。未获取到返回值。");
                throw new Exception("Token获取异常。未获取到返回值。");
            }
            return result;

        }
        private static string GetEncodedCredentials()
        {
            string m_Username = "username";
            string m_Password = "Password@123";
            string mergedCredentials = string.Format("{0}:{1}", m_Username, m_Password);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

    }
}
