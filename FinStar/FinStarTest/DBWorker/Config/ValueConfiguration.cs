using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace DBWorker.Config
{
    public class ValueConfiguration : IEntityTypeConfiguration<ValueSet>
    {
        public void Configure(EntityTypeBuilder<ValueSet> builder)
        {
            builder.HasKey(b => b.OrderNumber);
        }
    }
}
