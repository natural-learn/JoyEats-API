using JoyEats.Models;

namespace JoyEats.Core.VO.Dish
{
    public class DishVo
    {
        public long Id { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 菜品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 0 停售 1 起售
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 菜品关联的口味
        /// </summary>
        public List<DishFlavor> Flavors { get; set; } = new List<DishFlavor>();
    }
}
