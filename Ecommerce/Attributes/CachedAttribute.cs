using Ecommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Ecommerce.APIs.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
      
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

        
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

       
            try
            {
              
                var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

                if (cachedResponse != null)
                {
                  
                    var contentResult = new ContentResult
                    {
                        Content = cachedResponse,
                        ContentType = "application/json",
                        StatusCode = 200
                    };
                    context.Result = contentResult;
                    return;
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"[Cache Error]: Failed to get cache for key {cacheKey}. Exception: {ex.Message}");
            }

        
            var executedContext = await next();

    
            try
            {
              
                if (executedContext.Result is OkObjectResult okObjectResult)
                {
                    await cacheService.CacheResponseAsync(
                        cacheKey,
                        okObjectResult.Value!,
                        TimeSpan.FromSeconds(_timeToLiveSeconds)
                    );
                }
            }
            catch (Exception ex)
            {
             
                Console.WriteLine($"[Cache Error]: Failed to set cache for key {cacheKey}. Exception: {ex.Message}");
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }

    }
}
