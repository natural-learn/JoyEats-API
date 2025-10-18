using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Configs
{
    public class DishFlavorConfig : IEntityTypeConfiguration<DishFlavor>
    {
        public void Configure(EntityTypeBuilder<DishFlavor> builder)
        {
            builder.ToTable(nameof(DishFlavor));

            builder.HasKey(df => df.Id);
            builder.Property(df => df.Id).ValueGeneratedOnAdd();

            builder.Property(df => df.Name).HasMaxLength(32);
            builder.Property(df => df.Value).HasMaxLength(255);

            //List<DishFlavor> dishFlavors = new List<DishFlavor>()
            //{
            //    new DishFlavor() { Id = 1, DishId = 10,Name = "甜味",Value = "[\"无糖\",\"少糖\",\"半糖\",\"多糖\",\"全糖\"]"},
            //    new DishFlavor() { Id = 2, DishId = 7,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 3, DishId = 7,Name = "温度",Value = "[\"热饮\",\"常温\",\"去冰\",\"少冰\",\"多冰\"]"},
            //    new DishFlavor() { Id = 4, DishId = 6,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 5, DishId = 6,Name = "辣度",Value = "[\"不辣\",\"微辣\",\"中辣\",\"重辣\"]"},
            //    new DishFlavor() { Id = 6, DishId = 5,Name = "辣度",Value = "[\"不辣\",\"微辣\",\"中辣\",\"重辣\"]"},
            //    new DishFlavor() { Id = 7, DishId = 5,Name = "甜味",Value = "[\"无糖\",\"少糖\",\"半糖\",\"多糖\",\"全糖\"]"},
            //    new DishFlavor() { Id = 8, DishId = 2,Name = "甜味",Value = "[\"无糖\",\"少糖\",\"半糖\",\"多糖\",\"全糖\"]"},
            //    new DishFlavor() { Id = 9, DishId = 4,Name = "甜味",Value = "[\"无糖\",\"少糖\",\"半糖\",\"多糖\",\"全糖\"]"},
            //    new DishFlavor() { Id = 10, DishId = 3,Name = "甜味",Value = "[\"无糖\",\"少糖\",\"半糖\",\"多糖\",\"全糖\"]"},
            //    new DishFlavor() { Id = 11, DishId = 3,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 12, DishId = 7,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 13, DishId = 7,Name = "辣度",Value = "[\"不辣\",\"微辣\",\"中辣\",\"重辣\"]"},
            //    new DishFlavor() { Id = 14, DishId = 6,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 15, DishId = 6,Name = "辣度",Value = "[\"不辣\",\"微辣\",\"中辣\",\"重辣\"]"},
            //    new DishFlavor() { Id = 16, DishId = 8,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 17, DishId = 8,Name = "辣度",Value = "[\"不辣\",\"微辣\",\"中辣\",\"重辣\"]"},
            //    new DishFlavor() { Id = 18, DishId = 9,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\"]"},
            //    new DishFlavor() { Id = 19, DishId = 11,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 20, DishId = 12,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 21, DishId = 15,Name = "忌口",Value = "[\"不要葱\",\"不要蒜\",\"不要香菜\",\"不要辣\"]"},
            //    new DishFlavor() { Id = 22, DishId = 21,Name = "辣度",Value = "[\"不辣\",\"微辣\",\"中辣\",\"重辣\"]"},
            //    new DishFlavor() { Id = 23, DishId = 22,Name = "辣度",Value = "[\"不辣\",\"微辣\",\"中辣\",\"重辣\"]"},
            //    new DishFlavor() { Id = 24, DishId = 20,Name = "辣度",Value = "[\"不辣\",\"微辣\",\"中辣\",\"重辣\"]"},
            //};
            //builder.HasData(dishFlavors);

        }
    }
}
