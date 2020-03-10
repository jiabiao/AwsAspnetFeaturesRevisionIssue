using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace  AwsAspnetFeaturesRevisionIssue
{

    public class LambdaEntryPoint :
        Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .UseStartup<Startup>()
                .ConfigureServices(service => service.AddHttpContextAccessor())
                .ConfigureLogging((ctx, logging) =>
                {
                    logging.ClearProviders();

                    var singleton = ServiceDescriptor.Singleton<ILoggerProvider, TestLoggerProvider>(
                          serviceProvider => new TestLoggerProvider(serviceProvider));

                    logging.Services.Add(singleton);
                });
        }
    }
}
