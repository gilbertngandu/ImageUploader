using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Net;

namespace ImageUploader.Common.Extension
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ConcurrentDictionary<string, int> requestCounts = new ConcurrentDictionary<string, int>();
        private readonly int limit;

        public RateLimitMiddleware(RequestDelegate next, int limit)
        {
            this.next = next;
            this.limit = limit;
        }

        public async Task Invoke(HttpContext context)
        {
            string? identifier = context.Connection.RemoteIpAddress?.ToString();

            if(identifier == null) { return; }

            string key = $"{identifier}:{DateTime.UtcNow.Minute}";

            // Increment request count for this minute
            int count = requestCounts.AddOrUpdate(key, 1, (k, currentCount) => currentCount + 1);

            // Check if rate limit exceeded
            if (count > limit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded");
                return;
            }

            await next(context);
        }
    }
}
