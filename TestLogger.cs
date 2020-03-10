using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AwsAspnetFeaturesRevisionIssue
{
    public class TestLogger : ILogger
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _name;

        public TestLogger(IServiceProvider serviceProvider, string name)
        {
            _serviceProvider = serviceProvider;
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var accessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();

            var info = string.Empty;
            if (accessor.HttpContext != null)
            {
                //for LambdaEntryPoint, the Features.Revision always be 0
                // When the Logger<APIGatewayProxyFunction> logs somethings, because the `RequestServicesContainerMiddleware` has not been invoked. So the HttpContext.RequestServices is null. . 
                // Then the DefaultHttpContext caches null (by the FeatureReferences), and the cached revision is 0.
                // All subsequent access to HttpContext.RequestServices will get null. because the the revision is not changed.(always 0) 
                int pre = accessor.HttpContext.Features.Revision;
                var services = accessor.HttpContext.RequestServices;
                int post = accessor.HttpContext.Features.Revision;
                info = $"Features Revision:{pre} --> {post}, services :{services?.ToString() ?? "null"}";
            }


            var msg = formatter == null ? state.ToString() : formatter(state, exception);

            Console.Write($"{_name} {info}\n {msg}");
        }
    }
}
