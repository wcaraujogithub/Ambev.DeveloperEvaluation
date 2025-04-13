using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IdempotentAttribute : TypeFilterAttribute
    {
        public IdempotentAttribute(int expireHours = 24)
               : base(typeof(IdempotentFilterImpl))
        {
            Arguments = new object[] { expireHours };
        }
        private class IdempotentFilterImpl : IAsyncActionFilter
        {
            private readonly IMemoryCache _cache;
            private readonly int _expireHours;

            public IdempotentFilterImpl(IMemoryCache cache, int expireHours)
            {
                _cache = cache;
                _expireHours = expireHours;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var idempotencyKey = context.HttpContext.Request.Headers["Idempotency-Key"].FirstOrDefault();

                if (string.IsNullOrWhiteSpace(idempotencyKey))
                {
                    context.Result = new BadRequestObjectResult("Missing Idempotency-Key header");
                    return;
                }

                if (_cache.TryGetValue(idempotencyKey, out var cachedResult))
                {
                    context.Result = (IActionResult)cachedResult!;
                    return;
                }

                var executedContext = await next();

                if (executedContext.Result is ObjectResult objectResult)
                {
                    _cache.Set(idempotencyKey, objectResult, TimeSpan.FromHours(_expireHours));
                }
            }
        }

    }

}
