using Volo.Abp;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Modularity;

namespace Evo.Scm;

[DependsOn(
    typeof(AbpBackgroundWorkersQuartzModule)
)]
public class ScmWorkersModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }

    public override async void OnApplicationInitialization(
        ApplicationInitializationContext context)
    {
      
    }
}