using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDDH.Utility.Http;
using System.Diagnostics;

namespace SDDH.Test.UtilityTest
{
    [TestClass]
    public class HttpClientTest
    {
        [TestMethod]
        public void PostResponse()
        {
            try
            {
                //string url = "http://172.17.0.120:8042/api/Task/PulishDemo";
                //string param = @"{""Data"":{""Dep"":""PEK"",""Arr"":""TNA"",""DepDate"":""2018-06-23"",""PassengerType"":1,""Priority"": 8},""RequestKey"":""HttpClientTest""}";
                string url = "http://172.17.0.120:8041/Api/AirFlightShop/GetIBEPlusPrice";
                string param = "{\"AppID\":\"100611\",\"RequestKey\":\"0aee3ea7-3713-4560-a56f-b6dc0e6c49ba\",\"Data\":{\"OfficeNo\":\"SHA666\",\"Dep\":\"SHA\",\"Arr\":\"PEK\",\"DepDate\":\"2018-06-22T00:00:00\",\"PassengerType\":1}}";
                string rescode = "";
                var response = HttpClientHelper.PostResponse(url, param, out rescode);
            }
            catch (Exception ex)
            {
                var error = ex;
            }
        }


        [TestMethod]
        public void PostResponseAsync()
        {
            try
            {
                //string url = "http://172.17.0.120:8042/api/Task/PulishDemo";
                //string param = @"{""Data"":{""Dep"":""PEK"",""Arr"":""TNA"",""DepDate"":""2018-06-23"",""PassengerType"":1,""Priority"": 8},""RequestKey"":""HttpClientTest""}";
                string url = "http://172.17.0.120:8041/Api/AirFlightShop/GetIBEPlusPrice";
                string param = "{\"AppID\":\"100611\",\"RequestKey\":\"0aee3ea7-3713-4560-a56f-b6dc0e6c49ba\",\"Data\":{\"OfficeNo\":\"SHA666\",\"Dep\":\"NKG\",\"Arr\":\"CAN\",\"DepDate\":\"2018-06-24T00:00:00\",\"PassengerType\":1}}";
                string rescode = "";
                //HttpClientHelper.PostResponseAsync(url, param).Wait();
                Stopwatch sw1 = new Stopwatch();
                sw1.Start();
                string response1 = HttpClientHelper.PostResponseAsync(url, param).Result;
                sw1.Stop();
                var time1 = sw1.ElapsedMilliseconds;
                Stopwatch sw2 = new Stopwatch();
                sw2.Start();
                var response = HttpClientHelper.PostResponse(url, param, out rescode);
                sw2.Stop();
                var time2 = sw2.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                var error = ex;
            }
        }

    }
}
