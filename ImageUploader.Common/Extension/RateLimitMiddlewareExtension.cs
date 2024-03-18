using Microsoft.AspNetCore.Builder;

namespace ImageUploader.Common.Extension
{
    public static class RateLimitMiddlewareExtension
    {
        public static IApplicationBuilder UseRateLimit(this IApplicationBuilder builder, int limit)
        {
            return builder.UseMiddleware<RateLimitMiddleware>(limit);
        }
    }
}
