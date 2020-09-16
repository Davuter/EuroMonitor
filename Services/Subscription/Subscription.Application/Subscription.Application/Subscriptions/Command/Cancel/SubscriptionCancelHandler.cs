using MediatR;
using Microsoft.EntityFrameworkCore;
using Subscription.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Subscriptions.Command.Cancel
{
    public class SubscriptionCancelHandler : IRequestHandler<SubscriptionCancelIn, SubscriptionCancelOut>
    {
        private readonly ISubscriptionDbContext _dbContext;

        public SubscriptionCancelHandler(ISubscriptionDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<SubscriptionCancelOut> Handle(SubscriptionCancelIn request, CancellationToken cancellationToken)
        {
            SubscriptionCancelOut response = new SubscriptionCancelOut();

            var subcription = GetSubscription(request);

            CheckSubscription(subcription, response);
            if (response.ResultCode != 0)
            {
                return response;
            }

            subcription.StatusId = 2;
            subcription.UnSubscriptionDate = DateTime.Now;
            _dbContext.Subscriptions.Attach(subcription);
            _dbContext.Entry(subcription);
            int affectedRow = _dbContext.SaveChanges();

            if (affectedRow > 0)
            {
                response.ResultCode = 0;
                response.ResultMessage = "Succesfull";
                response.Id = request.Id;
            }
            else
            {
                response.ResultCode = 99;
                response.ResultMessage = "Failure";
                response.Id = request.Id;
            }

            return response;
        }

        private Subsricption.Domain.Entities.Subscriptions GetSubscription(SubscriptionCancelIn request)
        {
            return  _dbContext.Subscriptions.FirstOrDefault(r => r.ProductId == request.ProductId && r.BuyerId == request.BuyerId && r.Id == request.Id);
        }

        private SubscriptionCancelOut CheckSubscription(Subsricption.Domain.Entities.Subscriptions subcription, SubscriptionCancelOut response)
        {

            if (subcription == null)
            {
                response.ResultCode = 1;
                response.ResultMessage = "Subscription not found";
                return response;
            }

            if (subcription.StatusId == 2)
            {
                response.ResultCode = 2;
                response.ResultMessage = "Your subscription is already passive";
                return response;
            }

            return response;
        }
    }
}
