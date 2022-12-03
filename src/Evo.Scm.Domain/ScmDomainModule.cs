using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace Evo.Scm;

[DependsOn(
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpBackgroundJobsDomainModule),
    //typeof(AbpTenantManagementDomainModule),
    typeof(AbpEmailingModule)
)]
public class ScmDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    
    }
}
