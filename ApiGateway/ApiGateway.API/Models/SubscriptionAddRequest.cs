using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Models
{
    public class SubscriptionAddRequest
    {
        public string BuyerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
