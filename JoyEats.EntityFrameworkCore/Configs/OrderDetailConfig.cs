using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyEats.EntityFrameworkCore.Configs
{
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable(nameof(OrderDetail));

            builder.HasKey(od => od.Id);
            builder.Property(od => od.Id).ValueGeneratedOnAdd();

            builder.Property(od => od.Name).HasMaxLength(32);
            builder.Property(od => od.OrderId).IsRequired();
            builder.Property(od => od.DishFlavor).HasMaxLength(50);
            builder.Property(od => od.Number).IsRequired().HasDefaultValue(1);
            builder.Property(od => od.Amount).HasColumnType("decimal(10,2)");

        }
    }
}
