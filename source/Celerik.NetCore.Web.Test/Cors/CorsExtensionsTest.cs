using Celerik.NetCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Celerik.NetCore.Web.Test
{
    [TestClass]
    public class CorsExtensionsTest : WebBaseTest
    {
        [TestMethod]
        public void CorsExtensions_GetCorsConfig_Disabled()
        {
            var config = GetService<IConfiguration>();
            var corsConfig = config.GetCorsConfig();

            Assert.AreEqual(CorsPolicy.Disabled, corsConfig.Policy);
            CollectionAssert.AreEqual(new string[0], corsConfig.Origins);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void CorsExtensions_GetCorsConfig_InvalidPolicy()
        {
            var config = GetService<IConfiguration>();
            config["Cors:PolicyName"] = "QueBuenoUnasSalchipapas";
            config.GetCorsConfig();
        }

        [TestMethod]
        public void CorsExtensions_GetCorsConfig_AllowAnyOrigin()
        {
            var config = GetService<IConfiguration>();
            config["Cors:PolicyName"] = "AllowAnyOrigin";
            var corsConfig = config.GetCorsConfig();

            Assert.AreEqual(CorsPolicy.AllowAnyOrigin, corsConfig.Policy);
            CollectionAssert.AreEqual(new string[0], corsConfig.Origins);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void CorsExtensions_GetCorsConfig_AllowSpecificOrigins_NoOrigins()
        {
            var config = GetService<IConfiguration>();
            config["Cors:PolicyName"] = "AllowSpecificOrigins";
            config.GetCorsConfig();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void CorsExtensions_GetCorsConfig_AllowSpecificOrigins_InvalidOrigin()
        {
            var config = GetService<IConfiguration>();
            config["Cors:PolicyName"] = "AllowSpecificOrigins";
            config["Cors:Origins"] = "QueBuenoUnTamalConChocolate";
            config.GetCorsConfig();
        }

        [TestMethod]
        public void CorsExtensions_GetCorsConfig_AllowSpecificOrigins_VaidOrigins()
        {
            var config = GetService<IConfiguration>();
            config["Cors:PolicyName"] = "AllowSpecificOrigins";
            config["Cors:Origins"] = "http://salchipapas.com,http://tamales.com";
            var corsConfig = config.GetCorsConfig();

            Assert.AreEqual(CorsPolicy.AllowSpecificOrigins, corsConfig.Policy);
            CollectionAssert.AreEqual(
                new string[] { "http://salchipapas.com", "http://tamales.com" },
                corsConfig.Origins
            );
        }
    }
}
