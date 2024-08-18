using Microsoft.AspNetCore.Http.Features;
using System.Net.WebSockets;
using static System.Net.Mime.MediaTypeNames;

namespace ServiceActivationTest.Tenant
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITenantServiceProvider _tenantServiceProvider;

        public TenantMiddleware(RequestDelegate next, ITenantServiceProvider tenantServiceProvider)
        {
            _next = next;
            _tenantServiceProvider = tenantServiceProvider;
        }

        #region old
        //public async Task InvokeAsync(HttpContext context)
        //{
        //    //// Extract tenant info from the request (e.g., from headers, query string, or path)
        //    var tenantId = context.Request.Headers["TenantId"].ToString();
        //    //var tenantInfo = GetTenantInfo(tenantId);
        //    var tenantInfo = new TenantInfo { TenantId = tenantId };

        //    if (tenantInfo != null)
        //    {
        //        // Resolve tenant-specific service provider and replace the request's service provider
        //        var tenantServiceProvider = _tenantServiceProvider.GetServiceProvider(tenantInfo);
        //        if (tenantServiceProvider != null)
        //        {
        //            //var currentServiceProvider = context.RequestServices;
        //            context.RequestServices = tenantServiceProvider;
        //        }
        //    }

        //    await _next(context);
        //}
        #endregion

        public async Task InvokeAsync(HttpContext context)
        {
            //// Extract tenant info from the request (e.g., from headers, query string, or path)
            var tenantId = context.Request.Headers["TenantId"].ToString();
            //var tenantInfo = GetTenantInfo(tenantId);
            var tenantInfo = new TenantInfo { TenantId = tenantId };
            var tenantServiceProvider = _tenantServiceProvider.GetServiceProvider(tenantInfo);

            if (tenantServiceProvider != null)
            {
                IServiceProvidersFeature existingFeature = null!;
                try
                {
                    existingFeature = context.Features.Get<IServiceProvidersFeature>()!;
                    context.Features.Set<IServiceProvidersFeature>(
                        new RequestServicesFeature(context, tenantServiceProvider.GetRequiredService<IServiceScopeFactory>()));
                    await _next.Invoke(context);
                }
                finally
                {
                    // Restore the original feature if it was replaced (in case it is used before the response ends)
                    context.Features.Set(existingFeature);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
