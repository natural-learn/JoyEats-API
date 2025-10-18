using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Configs
{
    public class OrdersConfig : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.ToTable(nameof(Orders));

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.Property(o => o.Number).HasMaxLength(50);
            builder.Property(o => o.Status).IsRequired().HasDefaultValue(1);
            builder.Property(o => o.UserId).IsRequired();
            builder.Property(o => o.AddressBookId).IsRequired();
            builder.Property(o => o.OrderTime).IsRequired();
            builder.Property(o => o.PayMethod).IsRequired().HasDefaultValue(1);
            builder.Property(o => o.PayStatus).IsRequired();
            builder.Property(o => o.Amount).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(o => o.Remark).HasMaxLength(100);
            builder.Property(o => o.Phone).HasMaxLength(11);
            builder.Property(o => o.UserName).HasMaxLength(32);
            builder.Property(o => o.Consignee).HasMaxLength(32);
            builder.Property(o => o.DeliveryStatus).IsRequired().HasDefaultValue(1);
            builder.Property(o => o.TablewareStatus).IsRequired().HasDefaultValue(1);
        }
    }
}
