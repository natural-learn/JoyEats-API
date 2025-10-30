namespace SkyTakeOut.Models
{
    /// <summary>
    /// 菜品
    /// </summary>
    public class Dish : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜品分类id
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 导航属性：一个菜品属于一个分类
        /// </summary>
        public Category Category { get; set; }

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

        public ICollection<SetmealDish> SetmealDishes { get; set; } = new List<SetmealDish>();
    }
}
