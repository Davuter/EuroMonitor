using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Application.Subscriptions.Query
{
    public class SubscriptionQueryIn : IRequest<SubscriptionQueryOut>
    {
        public string BuyerId { get; set; }
        public int? ProductId { get; set; }
        public int? Id { get; set; }
        public int? StatusId { get; set; }
    }
}
