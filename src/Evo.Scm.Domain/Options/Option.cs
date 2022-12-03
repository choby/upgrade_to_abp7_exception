

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Evo.Scm.Options
{
    public class Option : Entity<Guid>, IMultiTenant
    {
        
       
        public virtual Guid? TenantId { get; set; }

        protected Option()
        {
          
        }
       
        

       
    }
}
