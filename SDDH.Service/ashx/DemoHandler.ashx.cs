using SDDH.Utility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDDH.Service
{
    /// <summary>
    /// DemoHandler 的摘要说明
    /// </summary>
    public class DemoHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string requestStr = GetString.ConvertContextToString(context);
                if (string.IsNullOrEmpty(requestStr))
                {
                    context.Response.Write("fail");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("error");
            }
            context.Response.Write("success");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}