using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using PortalClickerApi.Database;
using Serilog;

namespace PortalClickerApi.Middleware
{
    public class DbTransactionMiddlware
    {
        private readonly RequestDelegate _next;

        public DbTransactionMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var db = context.RequestServices.GetService<DatabaseContext>();
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<NoTransactionAttribute>();

            if (attribute != null || context.Request.Path.StartsWithSegments("/swagger"))
            {
                Log.Information("NO TRANACTION");
                await _next.Invoke(context);
            }
            else
            {
                Log.Information("TRANACTION");
                await using var transaction = await db!.Database.BeginTransactionAsync();
                await _next(context);
                await db.SaveChangesAsync();
                await transaction.CommitAsync();
                Log.Information("TRANACTION COMPLETE");
            }
        }
    }
}
