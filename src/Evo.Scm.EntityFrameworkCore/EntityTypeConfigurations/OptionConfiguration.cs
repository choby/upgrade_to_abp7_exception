
using Evo.Scm.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evo.Scm.EntityTypeConfigurations
{
    internal class OptionConfiguration: IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.ToTable(EvoScmDbProperties.DbTablePrefix + "Options", EvoScmDbProperties.DbSchema);
          

           
        }
    }

    
}
