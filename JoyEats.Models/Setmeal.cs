namespace JoyEats.Models
{
    /// <summary>
    /// 套餐
    /// </summary>
    public class Setmeal : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 菜品分类id
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 导航属性：一个套餐属于一个分类
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 套餐价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 售卖状态 0:停售 1:起售
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        public ICollection<SetmealDish> SetmealDishes { get; set; } = new List<SetmealDish>();
    }
}
