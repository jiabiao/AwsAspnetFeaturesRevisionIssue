using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
namespace AwsAspnetFeaturesRevisionIssue
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices(service => service.AddHttpContextAccessor())
                .ConfigureLogging((ctx, logging) =>
                {
                    logging.ClearProviders();

                    var singleton = ServiceDescriptor.Singleton<ILoggerProvider, TestLoggerProvider>(
                          serviceProvider => new TestLoggerProvider(serviceProvider));

                    logging.Services.Add(singleton);
                })
                .Build();
    }
}
