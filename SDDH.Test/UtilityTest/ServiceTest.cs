using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDDH.Test.ProxyerService;

namespace SDDH.Test.UtilityTest
{
    [TestClass]
    public class ServiceTest
    {
        [TestMethod]
        public void ProxyerServiceTest()
        {
            EtermProxyerServiceClient client = new EtermProxyerServiceClient();
            int providerId = 532494;
            string aircomCode = "MF";
            string ticketType = "BSP";
            bool result1 = client.IsOpenEtermProxyerWithProvider(providerId, aircomCode, ticketType);
            bool result2 = client.IsOpenEtermProxyer(aircomCode, ticketType);
        }
    }
}
