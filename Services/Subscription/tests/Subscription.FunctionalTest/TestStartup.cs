using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Subsription.API;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Subscription.FunctionalTest
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration env) : base(env)
        {
        }

        //public override IServiceProvider ConfigureServices(IServiceCollection services)
        //{
        //    // Added to avoid the Authorize data annotation in test environment. 
        //    // Property "SuppressCheckForUnhandledSecurityMetadata" in appsettings.json
        //    services.Configure<RouteOptions>(Configuration);
        //    return base.ConfigureServices(services);
        //}

        protected override void ConfigureAuth(IApplicationBuilder app)
        {
          app.UseMiddleware<SubscriptionAuthorizeMiddleware>();        
        }
        class SubscriptionAuthorizeMiddleware
        {
            private readonly RequestDelegate _next;
            public SubscriptionAuthorizeMiddleware(RequestDelegate rd)
            {
                _next = rd;
            }

            public async Task Invoke(HttpContext httpContext)
            {
                var identity = new ClaimsIdentity("cookies");
                identity.AddClaim(new Claim("sub", "4611ce3f-380d-4db5-8d76-87a8689058ed"));
                httpContext.User.AddIdentity(identity);
                await _next.Invoke(httpContext);
            }
        }
    }
}
