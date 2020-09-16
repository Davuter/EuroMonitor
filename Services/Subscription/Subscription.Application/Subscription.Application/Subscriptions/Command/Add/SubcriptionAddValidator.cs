using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Application.Subscriptions.Command.Add
{
    public class SubcriptionAddValidator :AbstractValidator<SubscriptionAddIn>
    {
        public SubcriptionAddValidator()
        {
            RuleFor(k => k.BuyerId).NotEmpty().NotNull();
            RuleFor(k => k.ProductId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
