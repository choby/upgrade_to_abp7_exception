using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Evo.Scm.Data;

/* This is used if database provider does't define
 * IScmDbSchemaMigrator implementation.
 */
public class NullScmDbSchemaMigrator : IScmDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
