using System;
using Microsoft.Extensions.Logging;

namespace AwsAspnetFeaturesRevisionIssue
{
    public class TestLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public TestLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new TestLogger(_serviceProvider, categoryName);
        }

        public void Dispose()
        {

        }
    }
}
