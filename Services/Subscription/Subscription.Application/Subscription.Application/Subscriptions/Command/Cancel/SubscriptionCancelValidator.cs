using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Application.Subscriptions.Command.Cancel
{
    public class SubscriptionCancelValidator : AbstractValidator<SubscriptionCancelIn>
    {
        public SubscriptionCancelValidator()
        {
            RuleFor(r => r.BuyerId).NotNull();
            RuleFor(r => r.ProductId).NotNull().GreaterThan(0);
        }
    }
}
