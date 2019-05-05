using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SDDH.Utility.Common
{
    public class GetString
    {
        /// <summary>
        /// 请求内容转字符串
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns></returns>
        public static string ConvertContextToString(HttpContext context)
        {
            var requestString = string.Empty;
            var s = context.Request.InputStream;
            byte[] bytes = new byte[s.Length];
            int numBytesToRead = (int)s.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to 10.
                int n = s.Read(bytes, numBytesRead, 1024);
                // The end of the file is reached.
                if (n == 0)
                {
                    break;
                }
                numBytesRead += n;
                numBytesToRead -= n;
            }
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
