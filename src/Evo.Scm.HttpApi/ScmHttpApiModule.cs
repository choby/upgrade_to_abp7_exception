
using Volo.Abp.Modularity;

namespace Evo.Scm;

[DependsOn(
    typeof(ScmApplicationContractsModule)
    )]
public class ScmHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }

   
}
