
using Microsoft.Extensions.Options;
using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Evo.Scm.Workers;

public class JstSyncWorker: QuartzBackgroundWorkerBase
{
   
    private CurrentTenant _currentTenant;
    private AbpDefaultTenantStoreOptions _tenantStoreOptions;
   
    private ICurrentPrincipalAccessor _currentPrincipalAccessor;
    public JstSyncWorker(CurrentTenant currentTenant,
        IOptions<AbpDefaultTenantStoreOptions> tenantStoreOptions,
       
        ICurrentPrincipalAccessor currentPrincipalAccessor) 
    {
      
        JobDetail = JobBuilder.Create<JstSyncWorker>().WithIdentity(nameof(JstSyncWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(JstSyncWorker))
            .WithCronSchedule("0 0/5 * * * ?") 
            //.StartAt(DateTimeOffset.Now.AddSeconds(10))
            .Build();
        
        
    }


    public override async Task Execute(IJobExecutionContext context)
    {
        
        
    }
}