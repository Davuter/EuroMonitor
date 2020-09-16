using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Application.Subscriptions.Command.Cancel
{
    public class SubscriptionCancelOut
    {
        public int Id { get; set; }
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }
}
