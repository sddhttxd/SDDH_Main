using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Zip
{
    /// <summary>
    /// 在线压缩/解压
    /// </summary>
    public class ZipHelper
    {
        /// <summary>
        /// 在线压缩文本(文件)
        /// </summary>
        public void DoZip()
        {
            try
            {
                #region  检查路径、保存文本
                string filePath = "";//文本文件目录
                string jsonFile = string.Format("{0}\\{1}.txt", filePath, "文件名");//文件地址
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (File.Exists(jsonFile))
                {
                    File.Delete(jsonFile);
                }
                using (FileStream fs = new FileStream(jsonFile, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine("文本内容");
                    }
                }
                #endregion

                #region 压缩打包、备份更新、删除文本
                if (File.Exists(jsonFile))
                {
                    string zipFile = string.Format("{0}\\{1}.zip", filePath, "文件名");
                    string bakFile = string.Format("{0}\\{1}_bak.zip", filePath, "文件名");
                    //Process.Info(username, requestkey, LogType.DoZipPolicy, "ZipJsonFile", "", "zipFile:" + zipFile, "压缩包路径");
                    if (File.Exists(zipFile))
                    {
                        if (File.Exists(bakFile))
                        {
                            File.Delete(bakFile);
                        }
                        File.Move(zipFile, bakFile);
                    }
                    using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFile)))
                    {
                        zipStream.SetLevel(6);
                        //zipStream.Password = "压缩密码";//可用可不用
                        using (FileStream fs = File.OpenRead(jsonFile))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            ZipEntry entry = new ZipEntry("文件名" + ".txt");
                            entry.Size = fs.Length;
                            entry.DateTime = DateTime.Now;
                            fs.Close();
                            zipStream.PutNextEntry(entry);
                            zipStream.Write(buffer, 0, buffer.Length);
                        }
                        zipStream.Finish();
                        zipStream.Close();
                    }
                    File.Delete(jsonFile);
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 在线解压成文本(文件)
        /// </summary>
        public void UnZip()
        {
            string stringData = "";
            try
            {
                #region 下载
                string zipPath = ""; //解压文件目录
                if (!Directory.Exists(zipPath))
                {
                    Directory.CreateDirectory(zipPath);
                }
                string zipUrl = ""; //压缩文件下载地址
                string urlname = zipUrl.Substring(zipUrl.LastIndexOf('/') + 1);
                string zipfile = zipPath + "\\" + urlname;

                HttpWebRequest request = HttpWebRequest.Create(zipUrl) as HttpWebRequest;
                request.Method = "GET";
                request.ProtocolVersion = new Version(1, 1);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    stringData = string.Empty;//找不到则直接返回null
                }
                // 转换为byte类型
                System.IO.Stream stream = response.GetResponseStream();

                //创建本地文件写入流
                using (Stream fs = new FileStream(zipfile, FileMode.Create))
                {
                    byte[] bArr = new byte[1024];
                    int size = stream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        fs.Write(bArr, 0, size);
                        size = stream.Read(bArr, 0, (int)bArr.Length);
                    }
                    fs.Close();
                }
                stream.Close();
                #endregion

                #region 解压
                if (File.Exists(zipfile))
                {
                    string txtFile = ""; //解压后的文件
                    using (FileStream fs = new FileStream(zipfile, FileMode.Open, FileAccess.Read))
                    {
                        using (ZipInputStream zipis = new ZipInputStream(fs))
                        {
                            zipis.Password = "解压密码";
                            ZipEntry entry;
                            while ((entry = zipis.GetNextEntry()) != null)
                            {
                                txtFile = zipPath + "\\" + entry.Name;
                                using (FileStream fileStream = new FileStream(txtFile, FileMode.Create, FileAccess.Write))
                                {
                                    int size = 2048;
                                    byte[] buffer = new byte[2048];
                                    while (size > 0)
                                    {
                                        size = zipis.Read(buffer, 0, buffer.Length);
                                        fileStream.Write(buffer, 0, size);
                                    }
                                }
                            }
                        }
                    }
                    using (StreamReader sr = new StreamReader(txtFile, Encoding.Default))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            stringData += stringData + line.ToString();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex; ;
            }
        }

    }
}
