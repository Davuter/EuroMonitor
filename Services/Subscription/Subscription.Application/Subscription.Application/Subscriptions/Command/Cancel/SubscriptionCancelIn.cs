using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Application.Subscriptions.Command.Cancel
{
    public class SubscriptionCancelIn : IRequest<SubscriptionCancelOut>
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public int ProductId { get; set; }
    }
}
