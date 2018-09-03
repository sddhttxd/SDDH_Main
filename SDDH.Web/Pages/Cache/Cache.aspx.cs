using Newtonsoft.Json;
using SDDH.Utility.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SDDH.Web.Pages
{
    public partial class Cache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            string key = txtKey.Text;
            string value = txtValue.Text;
            CacheManager.Instance.Redis.Set(key, value);
            CacheManager.Instance.Redis.Set(key + "1", value + "1", TimeSpan.FromMinutes(10));
            CacheManager.Instance.Redis.Set(key + "2", value + "2", DateTime.Now.AddMinutes(10));
            CacheManager.Instance.Redis.Set<User>(key + "3", new User { Age = 23, Name = value + "3" });
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            string key = txtKey.Text;
            string value = CacheManager.Instance.Redis.Get(key).ToString();
            string value1 = CacheManager.Instance.Redis.Get(key + "1").ToString();
            string value2 = CacheManager.Instance.Redis.Get(key + "2").ToString();
            User user = CacheManager.Instance.Redis.Get<User>(key + "3");
            txtResult.Value = value + "\r\n" + value1 + "\r\n" + value2 + "\r\n" + JsonConvert.SerializeObject(user);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            string key = txtKey.Text;
            bool result = CacheManager.Instance.Redis.Remove(key);
            txtResult.Value = result.ToString();
        }
    }

    public class User
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
}