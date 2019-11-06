using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
//using SDDH.Demo.CdiscountService;
using Cdiscount.Framework.Core.Communication.Messages;
using www.cdiscount.com;

namespace SDDH.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //GetAllowedCategoryTreeDemo2();
            GetTypeDemo();
        }

        public static void GetTypeDemo()
        {
            var type1 = typeof(TestObject1);
            var type2 = typeof(TestObject2);
            var demo1 = type1.GetType();
            var demo2 = type2.FullName;
        }


        private static IMarketplaceAPIService Client
        {
            get
            {
                var binding = new BasicHttpBinding()
                {
                    CloseTimeout = TimeSpan.FromSeconds(60),
                    OpenTimeout = TimeSpan.FromSeconds(60),
                    ReceiveTimeout = TimeSpan.FromSeconds(60),
                    SendTimeout = TimeSpan.FromSeconds(60),
                    MaxBufferSize = 655360000,
                    MaxBufferPoolSize = 5242880000,
                    MaxReceivedMessageSize = 655360000,
                    Security = new BasicHttpSecurity
                    {
                        Mode = BasicHttpSecurityMode.Transport
                    }
                };
                var channelFactory = new ChannelFactory<IMarketplaceAPIService>(binding, "https://wsvc.cdiscount.com/MarketplaceAPIService.svc");
                IMarketplaceAPIService client = channelFactory.CreateChannel();
                return client;
            }
        }

        public static void TaskDemo()
        {
            Console.WriteLine("MainThreadStart,ThreadId:" + Thread.CurrentThread.ManagedThreadId);
            int taskCount = 0;
            List<Task> tasks = new List<Task>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 10; i++)
            {
                Task task = Task.Run(() =>
                {
                    Console.WriteLine("SubThreadId:" + Thread.CurrentThread.ManagedThreadId);
                    Interlocked.Increment(ref taskCount);
                });

                //Task task1 = Task.Run<int>(() =>
                //{
                //    Interlocked.Increment(ref taskCount);
                //    return i;
                //});
            }
            Task.WhenAll(tasks).ContinueWith(o =>
            {
                watch.Stop();
                long usedTime = watch.ElapsedMilliseconds;
            });
            Console.WriteLine("MainThreadEnd,ThreadId:" + Thread.CurrentThread.ManagedThreadId);
        }


        public static void GetAllowedCategoryTreeDemo()
        {
            MarketplaceAPIServiceClient client = new MarketplaceAPIServiceClient();

            HeaderMessage headerMessage = new HeaderMessage()
            {
                Context = new ContextMessage()
                {
                    CatalogID = 1,
                    CustomerPoolID = 1,
                    SiteID = 100,
                },
                Localization = new LocalizationMessage()
                {
                    Country = Country.ER, //国家
                    Currency = Currency.Eur, //货币
                    DecimalPosition = 2, //小数位数
                    Language = Language.Fr, //语言
                },
                Security = new SecurityContext()
                {
                    UserName = "jushuitan-api",
                    IssuerID = null,
                    SessionID = null,
                    TokenId = "547ac026827f44a3abeddc20e971cc10", //"${#Project#648f57a70f82416dbc26d3aebe6b4058}",
                    SubjectLocality = null,
                },
                Version = "1.0",
            };
            CategoryTreeMessage result = client.GetAllowedCategoryTree(headerMessage);
            client.Close();
        }

        public static void GetAllowedCategoryTreeDemo2()
        {
            HeaderMessage headerMessage = new HeaderMessage()
            {
                Context = new ContextMessage()
                {
                    CatalogID = 1,
                    CustomerPoolID = 1,
                    SiteID = 100,
                },
                Localization = new LocalizationMessage()
                {
                    Country = Country.Fr, //国家
                    Currency = Currency.Eur, //货币
                    DecimalPosition = 2, //小数位数
                    Language = Language.Fr, //语言
                },
                Security = new SecurityContext()
                {
                    UserName = "ZZAMIER01-api",
                    IssuerID = null,
                    SessionID = null,
                    TokenId = "a1e21cec3ba645909d15af8282222c25",
                    SubjectLocality = null,
                },
                Version = "1.0",
            };
            CategoryTreeMessage result = Client.GetAllowedCategoryTree(headerMessage);
        }

        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="job"></param>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public static List<Product> GetProductList()
        {
            try
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                header.Add("version", "1.0");

                HeaderMessage headerMessage = new HeaderMessage()
                {
                    Context = new ContextMessage()
                    {
                        CatalogID = 1,
                        CustomerPoolID = 1,
                        SiteID = 100,
                    },
                    Localization = new LocalizationMessage()
                    {
                        Country = Country.ER, //国家
                        Currency = Currency.Eur, //货币
                        DecimalPosition = 2, //小数位数
                        Language = Language.Fr, //语言
                    },
                    Security = new SecurityContext()
                    {
                        UserName = "jushuitan",
                        IssuerID = "aca4a97ec36744df93894d265fc597cd",
                        SessionID = "aca4a97ec36744df93894d265fc597cd",
                        TokenId = "aca4a97ec36744df93894d265fc597cd",
                        SubjectLocality = null,
                    },
                    Version = "1.0",
                };

                ProductFilter productFilter = new ProductFilter
                {
                    CategoryCode = "123456",
                    ExtensionData = null
                };

                var GetProductList = new
                {
                    HeaderMessage = headerMessage,
                    ProductFilter = productFilter
                };

                var result = Client.GetProductList(headerMessage, productFilter);
                if (result.OperationSuccess && result.ProductList != null)
                {
                    return result.ProductList.ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }

    public class TestObject1
    {
        public int Id { get; set; }
    }
    public class TestObject2
    {
        public int Id { get; set; }
    }
    public class TestObject3
    {
        public int Id { get; set; }
    }
    public class TestObject4
    {
        public int Id { get; set; }
    }
}
