using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Application.Subscriptions.Query
{
    public class SubscriptionQueryOut  
    {
        public List<Subscriptions> Subscriptions { get; set; }
        public SubscriptionQueryOut()
        {
            Subscriptions = new List<Subscriptions>();
        }
    }

    public class Subscriptions
    {
        public int Id { get; set; }
        public DateTime SubscritionDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Status Status { get; set; }
        public DateTime? UnSubscritionDate { get; set; }
    }

    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
