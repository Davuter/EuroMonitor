using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Config
{
    public class AppSettings
    {
        public string CatalogServiceUrl { get; set; }
        public string SubscriptionServiceUrl { get; set; }
        public string IdentityServiceUrl { get; set; }
    }
}
