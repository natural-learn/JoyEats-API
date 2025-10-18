using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Configs
{
    public class ShoppingCartConfig : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.ToTable(nameof(ShoppingCart));

            builder.HasKey(sc => sc.Id);
            builder.Property(sc => sc.Id).ValueGeneratedOnAdd();

            builder.Property(sc => sc.Name).HasMaxLength(32);
            builder.Property(sc => sc.UserId).IsRequired();
            builder.Property(sc => sc.DishFlavor).HasMaxLength(50);
            builder.Property(sc => sc.Number).IsRequired().HasDefaultValue(1);
            builder.Property(sc => sc.Amount).IsRequired().HasColumnType("decimal(10,2)");
        }
    }
}
