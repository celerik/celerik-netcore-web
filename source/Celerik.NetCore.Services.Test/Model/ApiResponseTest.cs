using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Celerik.NetCore.Services.Test
{
    [TestClass]
    public class ApiResponseTest : ServiceBaseTest
    {
        [TestMethod]
        public void EmptyConstructor()
        {
            var response = new ApiResponse<object>();

            Assert.AreEqual(null, response.Data);
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(null, response.MessageType);
            Assert.AreEqual(true, response.Success);
        }

        [TestMethod]
        public void FillConstructor()
        {
            var response = new ApiResponse<object>("Chuck Norris knows Victoria's secret");

            Assert.AreEqual("Chuck Norris knows Victoria's secret", response.Data);
            Assert.AreEqual(null, response.Message);
            Assert.AreEqual(null, response.MessageType);
            Assert.AreEqual(true, response.Success);
        }

        [TestMethod]
        public void ToStringNullMessageType()
        {
            var response = new ApiResponse<object>();
            var toString = response.ToString();

            Assert.AreEqual(
                "{\"Data\":null,\"Message\":null,\"MessageType\":null,\"Success\":true}",
                toString
            );
        }

        [TestMethod]
        public void ToStringInfoMessageType()
        {
            var response = new ApiResponse<object>() { MessageType = ApiMessageType.Info };
            var toString = response.ToString();

            Assert.AreEqual(
                "{\"Data\":null,\"Message\":null,\"MessageType\":\"info\",\"Success\":true}",
                toString
            );
        }
    }
}
