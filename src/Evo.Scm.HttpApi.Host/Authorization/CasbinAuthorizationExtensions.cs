using Casbin.Adapter.EFCore;
using Microsoft.Extensions.DependencyInjection;
using NetCasbin;
using NetCasbin.Abstractions;
using Evo.Scm.Casbin;
using Microsoft.Extensions.Configuration;
using System.IO;
using Volo.Abp.VirtualFileSystem;
using Microsoft.Extensions.Hosting;

namespace Evo.Scm.Authorization
{
    public static class CasbinAuthorizationExtensions
    {
        public static void AddCasbinAuthorization(this IServiceCollection services)
        {
            services.AddSingleton(serviceProvider=> 
            {
                var context = serviceProvider.GetRequiredService<CasbinDbContext<int>>();
                //context.Database.EnsureCreated();
                var efCoreAdapter = new EFCoreAdapter<int>(context);

                var hostingEnvironment = services.GetHostingEnvironment();
                var modelPath = Path.Combine(hostingEnvironment.ContentRootPath, "Authorization/model.conf");

                var e = new Enforcer(modelPath, efCoreAdapter);
                e.LoadPolicy();
                e.EnableCache(true);
                return e;
            });
        }
    }
}
