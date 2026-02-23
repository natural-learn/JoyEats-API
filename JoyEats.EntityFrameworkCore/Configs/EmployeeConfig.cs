using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyEats.EntityFrameworkCore.Configs
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Name).HasMaxLength(32);
            builder.Property(e =>e.Username).HasMaxLength(32);
            builder.Property(e => e.Password).HasMaxLength(128);
            builder.HasIndex(e => e.Username).IsUnique();
            builder.Property(e => e.Phone).HasMaxLength(11).IsRequired();
            // 给手机号添加唯一约束
            builder.HasIndex(e => e.Phone).IsUnique();
            builder.Property(e => e.Sex).HasMaxLength(1);
            builder.Property(e => e.IdNumber).HasMaxLength(18);
            builder.Property(e => e.Status).HasDefaultValue(1);            
        }
    }
}
