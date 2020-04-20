using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Celerik.NetCore.Services.Test
{
    public interface ICalculatorService
    {
        ApiResponse<int> Plus(int a, int b);
    }

    public class CalculatorService : ApiService<ServiceBaseTest>, ICalculatorService
    {
        public CalculatorService(ApiServiceArgs<ServiceBaseTest> args)
            : base(args)
        {
        }

        public ApiResponse<int> Plus(int a, int b)
        {
            StartLog();

            Logger.LogDebug($"HttpContext: {HttpContext}");

            var c = a + b;
            var response = Ok<int>(c, ApiOperationType.Delete);

            EndLog();
            return response;
        }
    }

    [TestClass]
    public class ApiServiceTest : ServiceBaseTest
    {
        [TestMethod]
        public void CalculatorService()
        {
            var calculatorSvc = GetService<ICalculatorService>();
            Assert.IsInstanceOfType(calculatorSvc, typeof(CalculatorService));

            if (calculatorSvc is CalculatorService)
            {
                using (calculatorSvc as CalculatorService)
                {
                    var response = calculatorSvc.Plus(10, 5);

                    Assert.AreEqual(15, response.Data);
                    Assert.AreEqual(ApiMessageType.Success, response.MessageType);
                    Assert.AreEqual(true, response.Success);
                }
            }
        }
    }
}
