using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Subscription.Application.Subscriptions.Command.Add;
using Subscription.Application.Subscriptions.Command.Cancel;
using Subscription.Application.Subscriptions.Query;

namespace Subsription.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        IMediator _mediator;
        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Query")]
        [ProducesResponseType(typeof(SubscriptionQueryOut), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<SubscriptionQueryOut> Query(SubscriptionQueryIn subscriptionQueryIn)
        {
            return await _mediator.Send(subscriptionQueryIn);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<SubcriptionAddOut> Add(SubscriptionAddIn subscriptionAddIn)
        {
            return await _mediator.Send(subscriptionAddIn);
        }

        [HttpPost]
        [Route("Cancel")]
        public async Task<SubscriptionCancelOut> Cancel(SubscriptionCancelIn subscriptionAddIn)
        {
            return await _mediator.Send(subscriptionAddIn);
        }
    }
}