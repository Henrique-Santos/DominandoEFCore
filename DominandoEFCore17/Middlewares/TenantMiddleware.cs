using DominandoEFCore17.Extensions;
using DominandoEFCore17.Provider;

namespace DominandoEFCore17.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenant = context.RequestServices.GetRequiredService<TenantData>();

            tenant.TenantId = context.GetTenantId();

            await _next(context);
        }
    }
}