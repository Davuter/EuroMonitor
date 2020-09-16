using MediatR;
using Microsoft.EntityFrameworkCore;
using Subscription.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Subscriptions.Query
{
    public class SubscriptionQueryHandler : IRequestHandler<SubscriptionQueryIn, SubscriptionQueryOut>
    {
        public ISubscriptionDbContext _dbcontext;
        public SubscriptionQueryHandler(ISubscriptionDbContext dbContext)
        {
            _dbcontext = dbContext;
            
        }
        public async Task<SubscriptionQueryOut> Handle(SubscriptionQueryIn request, CancellationToken cancellationToken)
        {
            SubscriptionQueryOut response = new SubscriptionQueryOut();

            var subscriptons = _dbcontext.Subscriptions.Include("Status").Where(r => r.BuyerId == request.BuyerId);

            if (request.Id.GetValueOrDefault() > 0)
            {
                subscriptons = subscriptons.Where(r => r.Id == request.Id.GetValueOrDefault());
            }
            if (request.ProductId.GetValueOrDefault() > 0)
            {
                subscriptons = subscriptons.Where(r => r.ProductId == request.ProductId.GetValueOrDefault());
            }
            if (request.StatusId.GetValueOrDefault() > 0)
            {
                subscriptons = subscriptons.Where(r => r.StatusId == request.StatusId.GetValueOrDefault());
            }

            response = MaptoModel(subscriptons);

            return response;
        }

        private SubscriptionQueryOut MaptoModel(IQueryable<Subsricption.Domain.Entities.Subscriptions> subscriptions)
        {
            SubscriptionQueryOut subscriptionQueryOut = new SubscriptionQueryOut();

            foreach (var subscription in subscriptions)
            {
                Subscriptions subscriptionModel = new Subscriptions
                {
                    Id = subscription.Id,
                    ProductName = subscription.ProductName,
                    ProductId = subscription.ProductId,
                    Status = new Status
                    {
                        Id = subscription.Status.Id,
                        Name = subscription.Status.Name
                    },
                    SubscritionDate = subscription.SubscriptionDate,
                    UnSubscritionDate = subscription.UnSubscriptionDate
                };
                subscriptionQueryOut.Subscriptions.Add(subscriptionModel);

            }

            return subscriptionQueryOut;
        }
    }
}
