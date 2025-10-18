using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.Openid).HasMaxLength(45);
            builder.Property(u => u.Name).HasMaxLength(32);
            builder.Property(u => u.Phone).HasMaxLength(11);
            builder.Property(u => u.Sex).HasMaxLength(1);
            builder.Property(u => u.IdNumber).HasMaxLength(18);

        }
    }
}
