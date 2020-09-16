using Catalog.API;
using Catalog.API.Infrastructure.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace catalogs.functionaltests
{
    public class CatalogScenarioBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(CatalogScenarioBase))
              .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
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
           
            public static string Items()
            {
                return "api/catalog/items";
                
            }

            public static string ItemById(int id)
            {
                return $"api/catalog/item/{id}";
            }

        }
    }
}
