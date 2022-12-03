using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace Evo.Scm.EntityFrameworkCore;

[DependsOn(
    typeof(ScmDomainModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    //typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule) 
    )]
public class ScmEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        ScmEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ScmDbContext>(options =>
        {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: false);            
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.PreConfigure<ScmDbContext>(opts => {
                opts.DbContextOptions.UseLazyLoadingProxies();
            });
            
            /* The main point to change your DBMS.
             * See also ScmMigrationsDbContextFactory for EF Core tooling. */
            options.UseNpgsql(o => o.MinBatchSize(1).MaxBatchSize(100));//o=>o.MinBatchSize(1).MaxBatchSize(100)
        });
       

        //数据库提供程序在处理GUID时的行为有所不同,你应根据数据库提供程序进行设置. SequentialGuidType 有以下枚举成员:
        //SequentialAtEnd(default) 用于SQL Server.
        //SequentialAsString 用于MySQL和PostgreSQL.
        //SequentialAsBinary 用于Oracle.
        Configure<AbpSequentialGuidGeneratorOptions>(options =>
        {
            options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
        });

        
        context.Services.AddDefaultRepositories();
        
    }
}
