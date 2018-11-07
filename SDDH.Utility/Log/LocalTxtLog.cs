using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Log
{
    public class LocalTxtLog
    {
        private static StreamWriter streamWriter; //写文件  

        public static void WriteLog(string message)
        {
            try
            {
                //DateTime dt = new DateTime();
                //string directPath = ConfigurationManager.AppSettings["LogFilePath"].ToString().Trim();    //获得文件夹路径
                string directPath = System.String.Format(@"{0}\log\", System.AppDomain.CurrentDomain.BaseDirectory);
                if (!Directory.Exists(directPath))   //判断文件夹是否存在，如果不存在则创建
                {
                    Directory.CreateDirectory(directPath);
                }
                directPath += string.Format(@"\{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));
                if (streamWriter == null)
                {
                    streamWriter = !File.Exists(directPath) ? File.CreateText(directPath) : File.AppendText(directPath);    //判断文件是否存在如果不存在则创建，如果存在则添加。
                }
                streamWriter.WriteLine("***********************************************************************");
                streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                //streamWriter.WriteLine("输出信息：错误信息");
                if (message != null)
                {
                    streamWriter.WriteLine("日志信息：\r\n" + message);
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Flush();
                    streamWriter.Dispose();
                    streamWriter = null;
                }
            }
        }

        /// <summary>  
        /// 日志记录，写入txt文件  
        /// </summary>  
        /// <param name="stateCode">错误状态码</param>  
        /// <param name="msg">记录内容</param>  
        /// <param name="saveFolder">文件保存的目录</param>  
        /// <returns></returns>  
        public static void AddLog(string stateCode, string msg, string saveFolder)
        {
            string tishiMsg = "";
            try
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = AppDomain.CurrentDomain.BaseDirectory + saveFolder;
                if (Directory.Exists(filePath) == false)
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileAbstractPath = filePath + "\\" + fileName + ".txt";
                FileStream fs = new FileStream(fileAbstractPath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入     
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                msg = time + "，" + msg + System.Environment.NewLine;

                sw.Write(msg);
                //清空缓冲区               
                sw.Flush();
                //关闭流               
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();

                tishiMsg = "写入日志成功";
            }
            catch (Exception ex)
            {
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                tishiMsg = "[" + datetime + "]写入日志出错：" + ex.Message;
            }
        }

        /// <summary>  
        /// 将日志记录换行  
        /// </summary>  
        /// <param name="saveFolder">文件保存的目录</param>  
        /// <param name="rows">换几行</param>  
        public static void AddLine(string saveFolder, int rows)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = AppDomain.CurrentDomain.BaseDirectory + saveFolder;
                if (Directory.Exists(filePath) == false)
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileAbstractPath = filePath + "\\" + fileName + ".txt";
                FileStream fs = new FileStream(fileAbstractPath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入    
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < rows; i++)
                {
                    sb.Append(System.Environment.NewLine);
                }
                string newline = sb.ToString();
                sw.Write(newline);
                //清空缓冲区               
                sw.Flush();
                //关闭流               
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string tishiMsg = "[" + datetime + "]写入日志出错：" + ex.Message;
            }
        }

    }
}
