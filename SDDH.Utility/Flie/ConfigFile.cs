using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Flie
{
    public class ConfigFile
    {

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddSetting(string key, string value)
        {
            ConfigurationManager.RefreshSection(key);// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
            value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Add(key, value);
                config.Save();
            }
        }


        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void EditSetting(string key, string value)
        {
            ConfigurationManager.RefreshSection(key);// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
            value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
                config.Save();
            }
        }

    }
}
