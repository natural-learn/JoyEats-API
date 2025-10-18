using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Configs
{
    public class AddressBookConfig : IEntityTypeConfiguration<AddressBook>
    {
        public void Configure(EntityTypeBuilder<AddressBook> builder)
        {
            builder.ToTable("address_book");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            builder.Property(a => a.UserId).IsRequired();
            builder.Property(a => a.Consignee).HasMaxLength(50);
            builder.Property(a => a.Sex).HasMaxLength(1);
            builder.Property(a => a.Phone).HasMaxLength(11).IsRequired();
            builder.Property(a => a.ProvinceCode).HasMaxLength(12);
            builder.Property(a => a.ProvinceName).HasMaxLength(32);
            builder.Property(a => a.CityCode).HasMaxLength(12);
            builder.Property(a => a.CityName).HasMaxLength(32);
            builder.Property(a => a.DistrictCode).HasMaxLength(12);
            builder.Property(a => a.DistrictName).HasMaxLength(32);
            builder.Property(a => a.Detail).HasMaxLength(200);
            builder.Property(a => a.Label).HasMaxLength(100);
            builder.Property(a => a.IsDefault).IsRequired().HasDefaultValue(0);

        }
    }
}
