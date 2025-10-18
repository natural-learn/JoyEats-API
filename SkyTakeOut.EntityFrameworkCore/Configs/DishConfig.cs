using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyTakeOut.Models;

namespace SkyTakeOut.EntityFrameworkCore.Configs
{
    public class DishConfig : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> builder)
        {
            builder.ToTable(nameof(Dish));  

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();

            builder.Property(d => d.Name).HasMaxLength(32);
            builder.Property(d => d.Price).HasColumnType("decimal(10,2)");
            builder.Property(d => d.Status).HasDefaultValue(1);

            //List<Dish> dishes = new List<Dish>()
            //{
            //    new Dish { Id = 1, Name = "王老吉", CategoryId = 1, Price = 6.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/41bfcacf-7ad4-4927-8b26-df366553a94c.png", Description = "", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 2, Name = "北冰洋", CategoryId = 1, Price = 4.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/4451d4be-89a2-4939-9c69-3a87151cb979.png", Description = "还是小时候的味道", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 3,Name = "雪花啤酒", CategoryId = 1, Price = 4.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/bf8cbfc1-04d2-40e8-9826-061ee41ab87c.png", Description = "", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 4,Name = "米饭", CategoryId = 2, Price = 2.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/76752350-2121-44d2-b477-10791c23a8ec.png", Description = "精选五常大米", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 5,Name = "馒头", CategoryId = 2, Price = 1.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/475cc599-8661-4899-8f9e-121dd8ef7d02.png", Description = "优质面粉", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 6,Name = "老坛酸菜鱼", CategoryId = 9, Price = 56.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/4a9cefba-6a74-467e-9fde-6e687ea725d7.png", Description = "原料：汤，草鱼，酸菜", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 7,Name = "经典酸菜鮰鱼", CategoryId = 9, Price = 66.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/5260ff39-986c-4a97-8850-2ec8c7583efc.png", Description = "原料：酸菜，江团，鮰鱼", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 8,Name = "蜀味水煮草鱼", CategoryId = 9, Price = 38.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/a6953d5a-4c18-4b30-9319-4926ee77261f.png", Description = "原料：草鱼，汤", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 9,Name = "清炒小油菜", CategoryId = 8, Price = 18.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/3613d38e-5614-41c2-90ed-ff175bf50716.png", Description = "原料：小油菜", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 10,Name = "蒜蓉娃娃菜", CategoryId = 8, Price = 18.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/4879ed66-3860-4b28-ba14-306ac025fdec.png", Description = "原料：蒜，娃娃菜", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 11,Name = "清炒西兰花", CategoryId = 8, Price = 18.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/e9ec4ba4-4b22-4fc8-9be0-4946e6aeb937.png", Description = "原料：西兰花", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 12,Name = "炝炒圆白菜", CategoryId = 8, Price = 18.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/22f59feb-0d44-430e-a6cd-6a49f27453ca.png", Description = "原料：圆白菜", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 13,Name = "清蒸鲈鱼", CategoryId = 7, Price = 98.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/c18b5c67-3b71-466c-a75a-e63c6449f21c.png", Description = "原料：鲈鱼", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 14,Name = "东坡肘子", CategoryId = 7, Price = 138.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/a80a4b8c-c93e-4f43-ac8a-856b0d5cc451.png", Description = "原料：猪肘棒", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 15,Name = "梅菜扣肉", CategoryId = 7, Price = 58.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/6080b118-e30a-4577-aab4-45042e3f88be.png", Description = "原料：猪肉，梅菜", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 16,Name = "剁椒鱼头", CategoryId = 7, Price = 66.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/13da832f-ef2c-484d-8370-5934a1045a06.png", Description = "原料：鲢鱼，剁椒", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 17,Name = "金汤酸菜牛蛙", CategoryId = 6, Price = 88.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/7694a5d8-7938-4e9d-8b9e-2075983a2e38.png", Description = "原料：鲜活牛蛙，酸菜", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 18,Name = "香锅牛蛙", CategoryId = 6, Price = 88.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/f5ac8455-4793-450c-97ba-173795c34626.png", Description = "配料：鲜活牛蛙，莲藕，青笋", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 19,Name = "馋嘴牛蛙", CategoryId = 6, Price = 88.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/7a55b845-1f2b-41fa-9486-76d187ee9ee1.png", Description = "配料：鲜活牛蛙，丝瓜，黄豆芽", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 20,Name = "草鱼2斤", CategoryId = 5, Price = 68.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/b544d3ba-a1ae-4d20-a860-81cb5dec9e03.png", Description = "原料：草鱼，黄豆芽，莲藕", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 21,Name = "江团鱼2斤", CategoryId = 5, Price = 119.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/a101a1e9-8f8b-47b2-afa4-1abd47ea0a87.png", Description = "配料：江团鱼，黄豆芽，莲藕", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 22,Name = "鮰鱼2斤", CategoryId = 5, Price = 72.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/8cfcc576-4b66-4a09-ac68-ad5b273c2590.png", Description = "原料：鮰鱼，黄豆芽，莲藕", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 23,Name = "鸡蛋汤", CategoryId = 10, Price = 4.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/c09a0ee8-9d19-428d-81b9-746221824113.png", Description = "配料：鸡蛋，紫菜", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //    new Dish { Id = 24,Name = "平菇豆腐汤", CategoryId = 10, Price = 6.00m, Image = "https://sky-itcast.oss-cn-beijing.aliyuncs.com/16d0a3d6-2253-4cfc-9b49-bf7bd9eb2ad2.png", Description = "配料：豆腐，平菇", Status = 1, CreateTime = DateTime.Now, UpdateTime= DateTime.Now, CreateUser = 1, UpdateUser = 1 },
            //};
            //builder.HasData(dishes);
        }
    }
}
