using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Evo.Scm.EntityFrameworkCore;

using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.Auditing;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Aliyun;
using Volo.Abp.Settings;
using Volo.Abp.MultiTenancy;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;

namespace Evo.Scm;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpBlobStoringAliyunModule),
    typeof(ScmHttpApiModule),
    typeof(ScmApplicationModule),
    typeof(ScmEntityFrameworkCoreModule),
    typeof(ScmWorkersModule)
)]
public class ScmHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        
         Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = true;
            options.EntityHistorySelectors.AddAllEntities();
        });

        ConfigureConventionalControllers();
        ConfigureAuthentication(context, configuration);
        ConfigureCache(configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureSwaggerServices(context, configuration, hostingEnvironment);
        ConfigureNotProdEnvAccess(context);
        ConfigureMultiTenants(context);
        ConfigureResponseCompression(context);
    }

    private void ConfigureResponseCompression(ServiceConfigurationContext context)
    {
        context.Services.AddResponseCompression(options=>
        {
            options.EnableForHttps = true;
        });
    }

    private void ConfigureMultiTenants(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        var hostingEnvironment = context.Services.GetHostingEnvironment();
      

    }
    private void ConfigureNotProdEnvAccess(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        if (!hostingEnvironment.IsProduction())
            context.Services.AddAlwaysAllowAuthorization();
    }

    private void ConfigureCache(IConfiguration configuration)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "Scm:"; });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                
                options.FileSets.ReplaceEmbeddedByPhysical<ScmDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Evo.Scm.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<ScmApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Evo.Scm.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<ScmApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Evo.Scm.Application"));
            });
        }

    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(ScmApplicationModule).Assembly, opts =>
            {
                opts.RootPath = "scm";
            });
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "scm_backend";
                });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration, IWebHostEnvironment env)
    {
        context.Services.AddSwaggerGen();
    }

   





  

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

       
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseMultiTenancy();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI();

        app.UseAuditing();

        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
        app.UseResponseCompression();
    }


    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        base.PostConfigureServices(context);
    }
}
