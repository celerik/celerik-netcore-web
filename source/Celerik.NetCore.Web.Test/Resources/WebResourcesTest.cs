using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Celerik.NetCore.Web.Test
{
    [TestClass]
    public class WebResourcesTest : WebBaseTest
    {
        [TestMethod]
        public void WebResources_Get()
        {
            var name = "The database was deleted!";
            var resource = WebResources.Get(name);
            Assert.AreEqual(name, resource);
        }

        [TestMethod]
        public void WebResources_GetWithArgs()
        {
            var name = "The {0} database couldn´t be deleted!";
            var args = "ChuckNorrisFacts";
            var resource = WebResources.Get(name, args);

            Assert.AreEqual("The ChuckNorrisFacts database couldn´t be deleted!", resource);
        }
    }
}
