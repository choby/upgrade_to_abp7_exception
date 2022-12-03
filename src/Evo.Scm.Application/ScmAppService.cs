
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;

namespace Evo.Scm;

[Authorize]
public abstract class ScmAppService : ApplicationService
{
    protected ScmAppService()
    {
       
    }
}
