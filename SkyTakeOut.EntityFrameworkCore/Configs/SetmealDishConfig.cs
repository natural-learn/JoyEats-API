using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Configs
{
    public class SetmealDishConfig : IEntityTypeConfiguration<SetmealDish>
    {
        public void Configure(EntityTypeBuilder<SetmealDish> builder)
        {
            builder.ToTable(nameof(SetmealDish));

            builder.HasKey(sd => sd.Id);
            builder.Property(sd => sd.Id).ValueGeneratedOnAdd();

            builder.Property(sd => sd.Name).HasMaxLength(32);
            builder.Property(sd => sd.Price).HasColumnType("decimal(10,2)");

            builder.HasOne(sd => sd.Setmeal)
                .WithMany(s => s.SetmealDishes)
                .HasForeignKey(sd => sd.SetmealId);

            builder.HasOne(sd => sd.Dish)
                .WithMany(d => d.SetmealDishes)
                .HasForeignKey(sd => sd.DishId);
        }
    }
}
