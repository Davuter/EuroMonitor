using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Application.Subscriptions.Command.Add
{
    public class SubscriptionAddIn : IRequest<SubcriptionAddOut>
    {
        public string BuyerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
