using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Uow;

namespace Evo.Scm.Workers;

public class FinancialDetailCreateWorker: QuartzBackgroundWorkerBase
{
    
  
    public FinancialDetailCreateWorker( )
    {
       

        AutoRegister = true;
        JobDetail = JobBuilder.Create<FinancialDetailCreateWorker>().WithIdentity(nameof(FinancialDetailCreateWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(FinancialDetailCreateWorker))
            .WithCronSchedule("0 0 4 5 * ?") 
            .Build();
    }

    [UnitOfWork]
    public override async Task Execute(IJobExecutionContext context)
    {
      
        
    }

}