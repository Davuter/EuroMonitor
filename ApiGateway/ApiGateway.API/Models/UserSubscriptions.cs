using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Models
{
    public class UserSubscriptions
    {
        public string BuyerId { get; set; }
        public List<Subscriptions> Subscriptions { get; set; }
        
    }


    public class Subscriptions
    {
        public int Id { get; set; }
        public DateTime SubscritionDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Status Status { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public DateTime? UnSubscritionDate { get; set; }

    }

    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
