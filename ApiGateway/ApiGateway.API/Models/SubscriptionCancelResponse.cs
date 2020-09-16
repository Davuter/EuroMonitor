using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Models
{
    public class SubscriptionCancelResponse
    {
        public int Id { get; set; }
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }
}
