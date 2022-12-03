using Evo.Scm.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp.Domain.Entities;
using AbpDbContextRegistrationOptions = Volo.Abp.EntityFrameworkCore.DependencyInjection.AbpDbContextRegistrationOptions;

namespace Evo.Scm.EntityFrameworkCore
{
    public class EfCoreRepositoryRegistrar : Volo.Abp.EntityFrameworkCore.DependencyInjection.EfCoreRepositoryRegistrar
    {
        public EfCoreRepositoryRegistrar(AbpDbContextRegistrationOptions options) : base(options)
        {
        }

        public override void AddRepositories()
        {
            foreach (var entityType in GetEntityType())
            {
                RegisterDefaultRepository(entityType);
            }
        }

        private IEnumerable<Type> GetEntityType()
        {
            return Assembly.GetAssembly(typeof(Option)).GetTypes()
             .Where(s => typeof(IEntity).IsAssignableFrom(s))
             .Select(s => s).ToList();
        }
    }

    public static class ServiceDynamicDbSet
    {
        public static void AddDefaultRepositories(this IServiceCollection services)
        {
            // 传递一个AbpCommonDbContextRegistrationOptions类型，便于RepositoryRegistrarBase基类属性注入
            var options = new AbpDbContextRegistrationOptions(typeof(ScmDbContext), services);

            // 我们上边自定义获取EntityType实现注入默认仓储
            new EfCoreRepositoryRegistrar(options).AddRepositories();
        }
    }
}
