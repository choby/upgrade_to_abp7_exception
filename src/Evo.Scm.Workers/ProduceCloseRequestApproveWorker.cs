
using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Evo.Scm.Workers;


public class ProduceCloseRequestApproveWorker: QuartzBackgroundWorkerBase
{
   
    public ProduceCloseRequestApproveWorker()
    {
       
        
        AutoRegister = true;
        JobDetail = JobBuilder.Create<ProduceCloseRequestApproveWorker>().WithIdentity(nameof(ProduceCloseRequestApproveWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(ProduceCloseRequestApproveWorker))
            .WithCronSchedule("0 0 2 * * ?") 
            .Build();
    }

    [UnitOfWork]
    public override async Task Execute(IJobExecutionContext context)
    {
       
    }

  
    
   
}