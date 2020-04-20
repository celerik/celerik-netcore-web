using Celerik.NetCore.Util;
using Microsoft.Extensions.DependencyInjection;

namespace Celerik.NetCore.Services.Test
{
    public class ServiceBaseTest : BaseTest
    {
        protected override void AddServices(IServiceCollection services)
        {
            services.AddCoreServices<ServiceBaseTest>((config, apiConfig) =>
                {
                    config["ServiceType"] = ApiServiceType.ServiceMock.GetDescription();
                })
                .AddAutomapper()
                .AddValidators(() =>
                {
                    services.AddValidator<PaginationRequest, PaginationRequestValidator<PaginationRequest>>();
                })
                .AddBusinesServices(() =>
                {
                    services.AddTransient<ICalculatorService, CalculatorService>();
                });
        }
    }
}
