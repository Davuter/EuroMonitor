using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Subsription.API.Filter
{
    public class ByPassAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public ByPassAuthMiddleware(RequestDelegate rd)
        {
            _next = rd;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var identity = new ClaimsIdentity("cookies");
            identity.AddClaim(new Claim("sub", "4611ce3f-380d-4db5-8d76-87a8689058ed"));
            httpContext.User.AddIdentity(identity);
            await _next.Invoke(httpContext);
        }
    }
}
