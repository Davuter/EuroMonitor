using ApiGateway.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Services
{
    public interface ISubscriptionService
    {
        Task<UserSubscriptions> GetUserSubscriptions(SubscriptionsRequest request);
        Task<SubcriptionAddResponse> AddUserSubscription(SubscriptionAddRequest request);
        Task<SubscriptionCancelResponse> CancelUserSubscription(SubscriptionCancelRequest request);
    }
}
