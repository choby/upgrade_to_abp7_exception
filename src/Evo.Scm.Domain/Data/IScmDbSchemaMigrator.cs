using System.Threading.Tasks;

namespace Evo.Scm.Data;

public interface IScmDbSchemaMigrator
{
    Task MigrateAsync();
}
