using JoyEats.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoyEats.EntityFrameworkCore.Configs
{
    public class SetmealConfig : IEntityTypeConfiguration<Setmeal>
    {
        public void Configure(EntityTypeBuilder<Setmeal> builder)
        {
            builder.ToTable(nameof(Setmeal));

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();

            builder.Property(s => s.Name).HasMaxLength(32).IsRequired();
            builder.Property(s => s.Price).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(s => s.Status).HasDefaultValue(1);

        }
    }
}
