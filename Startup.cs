using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AwsAspnetFeaturesRevisionIssue
{

    public class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(context =>
            {
                // LocalEntryPoint: services is never be null.
                // LambdaEntryPoint: services is null

                int pre = context.Features.Revision;
                var services = context.RequestServices;
                int post = context.Features.Revision;
                var info = $"Features Revision:{pre} --> {post}, services :{services?.ToString() ?? "null"}";

                return context.Response.WriteAsync(info);
            });
        }
    }
}
