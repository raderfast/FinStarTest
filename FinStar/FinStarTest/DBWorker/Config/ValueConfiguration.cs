using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBWorker.Config
{
    public class ValueConfiguration : IEntityTypeConfiguration<Models.Body>
    {
        public void Configure(EntityTypeBuilder<Models.Body> builder)
        {
        }
    }
}
