using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;

namespace SDDH.Utility.DataBase
{
    public static class SqlServerDemo
    {
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["SqlServerDemo"].ToString();

        public static string GetDataTest()
        {
            string result = "";
            try
            {
                //string sql = "SELECT * FROM dbo.Test WHERE Id IN ('+@Id+')";
                string sql = "EXEC('SELECT * FROM dbo.Test WHERE Id IN ('+@Id+') and info in('+@Info+')')";
                //DynamicParameters param = new DynamicParameters();
                //param.Add("Id", "1,2,3");
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var dbResult = conn.Query(sql, new { Id = "1,2,3", Info = "'test1','test2'" });
                    //var dbResult = conn.Query(sql, new { Id = "1,2,3" });
                    result = JsonConvert.SerializeObject(dbResult);
                }
            }
            catch (Exception ex)
            {
                var log = ex;
            }
            return result;
        }

    }
}
