using Celerik.NetCore.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Celerik.NetCore.Services.Test
{
    [TestClass]
    public class ApiExtensionsTest : ServiceBaseTest
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void GetServiceTypeUndefined()
        {
            var config = GetService<IConfiguration>();
            config["ServiceType"] = null;
            config.GetServiceType();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void GetServiceTypeEmpty()
        {
            var config = GetService<IConfiguration>();
            config["ServiceType"] = "";
            config.GetServiceType();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void GetServiceTypeInvalid()
        {
            var config = GetService<IConfiguration>();
            config["ServiceType"] = "ArnoldSchwarzeneggerAPI";
            config.GetServiceType();
        }

        [TestMethod]
        public void GetServiceTypeServiceEF()
        {
            var config = GetService<IConfiguration>();
            config["ServiceType"] = "ServiceEF";

            Assert.AreEqual(ApiServiceType.ServiceEF, config.GetServiceType());
        }

        [TestMethod]
        public void GetServiceTypeServiceHttp()
        {
            var config = GetService<IConfiguration>();
            config["ServiceType"] = "ServiceHttp";

            Assert.AreEqual(ApiServiceType.ServiceHttp, config.GetServiceType());
        }

        [TestMethod]
        public void GetServiceTypeServiceMock()
        {
            var config = GetService<IConfiguration>();
            config["ServiceType"] = "ServiceMock";

            Assert.AreEqual(ApiServiceType.ServiceMock, config.GetServiceType());
        }
    }
}
