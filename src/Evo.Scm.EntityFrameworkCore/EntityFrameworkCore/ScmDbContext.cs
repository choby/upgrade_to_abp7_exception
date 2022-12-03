
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace Evo.Scm.EntityFrameworkCore;


[ConnectionStringName("Default")]
public class ScmDbContext : AbpDbContext<ScmDbContext>
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    #endregion


    public ScmDbContext(DbContextOptions<ScmDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //加载当前程序集的所有实体配置
        //这一行代码不能放到OnModelCreating后面，否则ShouldFilterEntity等相关方法不会被执行
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);


        /* Include modules to your migration db context */


        //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
        //      .Where(x => !string.IsNullOrEmpty(x.Namespace))
        //      .Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
        //foreach (var type in typesToRegister)
        //{
        //    dynamic configurationInstance = Activator.CreateInstance(type);
        //    builder.ApplyConfiguration(configurationInstance
        //      );
        //}



        //builder.Entity<Author>(builder =>
        //{
        //    builder.ToTable("EvoAuthors");
        //    builder.ConfigureByConvention();
        //    builder.Property(x => x.Name)
        //       .IsRequired()
        //       .HasMaxLength(12);

        //    builder.HasIndex(x => x.Name);
        //});

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ScmConsts.DbTablePrefix + "YourEntities", ScmConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
   
    public ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();

   
    


    
}
