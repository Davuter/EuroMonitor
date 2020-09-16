using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Subsription.API;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Subscription.Application.Subscriptions.Query;

namespace Subscription.FunctionalTest
{
    public class BaseScenario
    {
  
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(BaseScenario))
                 .Location;


            var hostBuilder = new WebHostBuilder().UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                })
                .UseStartup<Startup>();


            var testServer = new TestServer(hostBuilder);


            return testServer;
        }

      
        public static class Post
        {
            public static string Query = "api/Subscription/Query/";
            public static string Add = "api/Subscription/Add/";
            public static string Cancel = "api/Subscription/Cancel/";
        }
    }
}
