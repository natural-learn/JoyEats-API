using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Configs
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(nameof(Category));

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Type).HasDefaultValue(null);
            builder.Property(c => c.Name).HasMaxLength(32).IsRequired();
            builder.Property(c => c.Sort).IsRequired().HasDefaultValue(0);
            builder.Property(c => c.Status).HasDefaultValue(null);
        }
    }
}
