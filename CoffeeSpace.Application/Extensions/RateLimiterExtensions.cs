using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.Application.Extensions;

public static class RateLimiterExtensions
{
    public const string BucketName = "TokenBucket";
    
    public static IServiceCollection AddBucketRateLimiter(this IServiceCollection services, int rejectionStatusCode)
    {
        return services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = rejectionStatusCode;

            options.AddTokenBucketLimiter("TokenBucket", limiterOptions =>
            {
                limiterOptions.TokenLimit = 20;
                limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
                limiterOptions.TokensPerPeriod = 5;
                limiterOptions.QueueLimit = 3;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
        });
    }
}