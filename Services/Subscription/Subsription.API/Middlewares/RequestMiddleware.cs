using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Subsription.API.Middlewares
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private readonly ILogger<RequestMiddleware> _logger;
        static readonly HashSet<string> HeaderWhitelist = new HashSet<string> {
            "Content-Type", "Content-Length", "User-Agent","Request-Id","Session-Id" };


        public RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //httpContext.Request.Headers

            var start = Stopwatch.GetTimestamp();
            var allowedHeaders = httpContext.Request.Headers.Where(h => HeaderWhitelist.Contains(h.Key));

            //LogContext.PushProperty("RequestSessionId", allowedHeaders. );
            var headerDictionary = allowedHeaders.ToDictionary(h => h.Key, h => h.Value.ToString());

            Log.ForContext("RequestHeaders", headerDictionary, destructureObjects: true)
           .Debug("Request information {RequestMethod} {RequestPath} information", httpContext.Request.Method, httpContext.Request.Path);

      

            double elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

            Log.Debug("Response information {RequestMethod} {RequestPath} {statusCode} {executedIn} in ms",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Response.StatusCode,
                elapsedMs);

            await _next(httpContext);
        }

        static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }

    public static class RequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}
