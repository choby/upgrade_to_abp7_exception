using Volo.Abp.Data;

namespace Evo.Scm;

public static class EvoScmDbProperties
{
    public static string DbTablePrefix { get; set; } = "xxxxx";
    public static string DbViewPrefix { get; set; } = "VIEW_";

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "xxxx";
}
