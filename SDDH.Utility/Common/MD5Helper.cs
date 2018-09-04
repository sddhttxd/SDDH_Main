using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Common
{
    /// <summary>
    /// MD5加密类
    /// </summary>
    public class MD5Helper
    {

        /// <summary>
        /// 32位MD5加密(大写)
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string MD5Encrypt32_utf8(string password)
        {
            DateTime date = DateTime.Now;
            //10分钟buffer，避免因服务器时间不一致导致密码不对
            if (date.Hour >= 23 && date.Minute > 50)
            {
                date = date.AddDays(1);
            }
            string pwd = "";
            MD5 md = MD5.Create(); //实例化一个md5对像
            byte[] s = md.ComputeHash(Encoding.UTF8.GetBytes(password));// 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X2"); //"X"和"X2"有区别，"X"两个0相连的时候可以会少一位
            }
            return pwd;
        }

    }
}
