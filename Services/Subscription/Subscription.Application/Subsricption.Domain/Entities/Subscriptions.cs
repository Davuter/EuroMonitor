using System;
using System.Collections.Generic;
using System.Text;

namespace Subsricption.Domain.Entities
{
    public class Subscriptions
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public DateTime? UnSubscriptionDate { get; set; }
    }
}
