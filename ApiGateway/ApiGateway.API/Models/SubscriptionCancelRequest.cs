using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Models
{
    public class SubscriptionCancelRequest
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public int ProductId { get; set; }
    }
}
