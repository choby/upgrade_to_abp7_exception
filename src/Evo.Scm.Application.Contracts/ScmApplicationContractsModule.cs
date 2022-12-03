using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Evo.Scm;

[DependsOn(
    typeof(AbpObjectExtendingModule),
    typeof(AbpFluentValidationModule)
)]
public class ScmApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ScmDtoExtensions.Configure();
    }
}
