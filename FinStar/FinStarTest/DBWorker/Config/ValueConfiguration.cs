using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBWorker.Config
{
    public class ValueConfiguration : IEntityTypeConfiguration<Models.Value>
    {
        public void Configure(EntityTypeBuilder<Models.Value> builder)
        {
            builder.HasKey(b => b.OrderNumber);
        }
    }
}
