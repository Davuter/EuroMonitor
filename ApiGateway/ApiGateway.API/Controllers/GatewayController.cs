using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.API.Models;
using ApiGateway.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GatewayController : ControllerBase
    {
        private readonly ICatalogServices _catologService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IIdentityService _identityService;
        public GatewayController(ICatalogServices catalogServices,ISubscriptionService subscriptionService, IIdentityService identityService)
        {
            _catologService = catalogServices;
            _subscriptionService = subscriptionService;
            _identityService = identityService;
        }
        [HttpGet]
        [Route("CatalogItems")]
        [AllowAnonymous]
        public async Task<List<CatalogItem>> GetCatalogItems()
        {
            return await _catologService.GetCatalogItems();
        }

        [HttpPost]
        [Route("UserSubscriptions")]
        public async Task<UserSubscriptions> GetUserSubscriptions(SubscriptionsRequest request)
        {
            var catalogItems = await _catologService.GetCatalogItems();
            var usersubsrictions = await _subscriptionService.GetUserSubscriptions(request);

            foreach (var usersubsriction in usersubsrictions.Subscriptions)
            {
                var catalogItem = catalogItems.FirstOrDefault(r => r.Id == usersubsriction.ProductId);
                usersubsriction.CatalogItem = catalogItem;
            }

            return usersubsrictions;
        }

        [HttpPost]
        [Route("CatalogItemsWithSubcripted")]
        public async Task<List<CatalogItem>> GetCatalogWithSubcripted(SubscriptionsRequest request)
        {
            var catalogItems = await _catologService.GetCatalogItems();
            var usersubsrictions = await _subscriptionService.GetUserSubscriptions(request);

            foreach (var catalogItem in catalogItems)
            {
                var subscription = usersubsrictions.Subscriptions.FirstOrDefault(r => r.ProductId == catalogItem.Id && r.Status.Id == 1);

                catalogItem.SubscriptedUser = subscription != null;
                catalogItem.SubscriptedId = subscription != null ? subscription.Id : 0;
            }

            return  catalogItems;
        }
        [HttpPost]
        [Route("AddUserSubscription")]
        public async Task<SubcriptionAddResponse> AddUserSubscription(SubscriptionAddRequest request)
        {
            return await _subscriptionService.AddUserSubscription(request);
        }
        [HttpPost]
        [Route("CancelUserSubscription")]
        public async Task<SubscriptionCancelResponse> CancelUserSubscription(SubscriptionCancelRequest request)
        {
            return await _subscriptionService.CancelUserSubscription(request);
        }
        [HttpGet]
        [Route("Token")]
        [AllowAnonymous]
        public async Task<TokenModel> GetToken(string userName,string password)
        {
            return await _identityService.GetToken(userName, password);
        }

        
    }
}