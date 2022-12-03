
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Evo.Scm;

[DependsOn(
    typeof(ScmDomainModule),
    typeof(ScmApplicationContractsModule)
    //typeof(AbpTenantManagementApplicationModule),
    )]
public class ScmApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ScmApplicationModule>();
        });
    }
}
