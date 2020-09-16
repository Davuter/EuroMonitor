using ApiGateway.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace apigateway.tests
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

        public static class Get
        {
            public static string Query = "api/Gateway/CatalogItems/";
            public static string Token(string username,string password)
            {
                return $"api/Gateway/Token?userName={username}&password={password}";
            }
         
        }

        public static class Post
        {
            public static string UserSubscriptions = "api/Gateway/UserSubscriptions/";
            public static string CatalogItemsWithSubcripted = "api/Gateway/CatalogItemsWithSubcripted/";
            public static string AddUserSubscription = "api/Gateway/AddUserSubscription/";
            public static string CancelUserSubscription = "api/Gateway/CancelUserSubscription/";
         
        }
    }
}
