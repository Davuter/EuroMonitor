using MediatR;
using Serilog;
using Subscription.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Subscriptions.Command.Add
{
    public class SubscriptionAddHandler : IRequestHandler<SubscriptionAddIn, SubcriptionAddOut>
    {
        private readonly ISubscriptionDbContext _dbContext;
        public SubscriptionAddHandler(ISubscriptionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SubcriptionAddOut> Handle(SubscriptionAddIn request, CancellationToken cancellationToken)
        {
            try
            {
                SubcriptionAddOut response = new SubcriptionAddOut();

                if (AlreadyExistSubscription(request))
                {
                    response.Id = 0;
                    return response;
                }
               
                Subsricption.Domain.Entities.Subscriptions subscription = new Subsricption.Domain.Entities.Subscriptions
                {
                    BuyerId = request.BuyerId,
                    ProductId = request.ProductId,
                    ProductName = request.ProductName,
                    SubscriptionDate = request.SubscriptionDate,
                    StatusId = 1 // Active
                };

                _dbContext.Subscriptions.Add(subscription);
                _dbContext.SaveChanges();

                response.Id = subscription.Id;
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw ex;
            }
          
        }

        private bool AlreadyExistSubscription(SubscriptionAddIn request)
        {
            return _dbContext.Subscriptions.Any(r => r.ProductId == request.ProductId && r.BuyerId == request.BuyerId && r.StatusId == 1);
        }
    }
}
