using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SDDH.Utility.Common
{
    /// <summary>
    /// 获取站点路径
    /// </summary>
    public class GetPath
    {

        /// <summary>
        /// 获取站点根目录
        /// </summary>
        /// <returns></returns>
        public string GetRootPath()
        {
            string path = "";
            try
            {
                if (HttpContext.Current != null)
                {
                    path = HttpContext.Current.Server.MapPath("/") + "Data";//有http请求
                    return path;
                }
                else //非web程序引用         
                {
                    path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                    return path;
                }
            }
            catch (Exception ex)
            {
                path = "/Data";
                throw ex;
            }
        }

    }
}
