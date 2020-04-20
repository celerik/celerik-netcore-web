using Celerik.NetCore.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Celerik.NetCore.Web.Test
{
    [TestClass]
    public class SwaggerExtensionsTest : WebBaseTest
    {
        [TestMethod]
        public void SwaggerExtensions_GetSwaggerConfig_IsEnabled_Undefined()
        {
            var config = GetService<IConfiguration>();
            var swaggerConfig = config.GetSwaggerConfig();

            Assert.AreEqual(false, swaggerConfig.IsEnabled);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void SwaggerExtensions_GetSwaggerConfig_IsEnabled_Invalid()
        {
            var config = GetService<IConfiguration>();
            config["Swagger:IsEnabled"] = "Chuck Norris counted to infinity. Twice.";
            config.GetSwaggerConfig();
        }

        [TestMethod]
        public void SwaggerExtensions_GetSwaggerConfig_IsEnabled_False()
        {
            var config = GetService<IConfiguration>();
            config["Swagger:IsEnabled"] = "false";
            var swaggerConfig = config.GetSwaggerConfig();

            Assert.AreEqual(false, swaggerConfig.IsEnabled);
        }

        [TestMethod]
        public void SwaggerExtensions_GetSwaggerConfig_IsEnabled_True()
        {
            var config = GetService<IConfiguration>();
            config["Swagger:IsEnabled"] = "true";
            config["Swagger:ApiVersion"] = "v.1";
            config["Swagger:ApiName"] = "Chuck Norris API";
            config["Swagger:JsonEndpoint"] = "/swagger/v1/chuck-norris.json";
            var swaggerConfig = config.GetSwaggerConfig();

            Assert.AreEqual(true, swaggerConfig.IsEnabled);
            Assert.AreEqual("v.1", swaggerConfig.ApiVersion);
            Assert.AreEqual("Chuck Norris API", swaggerConfig.ApiName);
            Assert.AreEqual("/swagger/v1/chuck-norris.json", swaggerConfig.JsonEndpoint);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void SwaggerExtensions_GetSwaggerConfig_ApiVersion_Missing()
        {
            var config = GetService<IConfiguration>();
            config["Swagger:IsEnabled"] = "true";
            config["Swagger:ApiName"] = "Chuck Norris API";
            config["Swagger:JsonEndpoint"] = "/swagger/v1/chuck-norris.json";
            config.GetSwaggerConfig();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void SwaggerExtensions_GetSwaggerConfig_ApiName_Missing()
        {
            var config = GetService<IConfiguration>();
            config["Swagger:IsEnabled"] = "true";
            config["Swagger:ApiVersion"] = "v.1";
            config["Swagger:JsonEndpoint"] = "/swagger/v1/chuck-norris.json";

            config.GetSwaggerConfig();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigException))]
        public void SwaggerExtensions_GetSwaggerConfig_JsonEndpoint_Missing()
        {
            var config = GetService<IConfiguration>();
            config["Swagger:IsEnabled"] = "true";
            config["Swagger:ApiVersion"] = "v.1";
            config["Swagger:ApiName"] = "Chuck Norris API";

            config.GetSwaggerConfig();
        }
    }
}
