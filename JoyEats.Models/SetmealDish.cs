namespace JoyEats.Models
{
    /// <summary>
    /// 套餐菜品关系
    /// </summary>
    public class SetmealDish
    {
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 套餐id
        /// </summary>
        public long? SetmealId { get; set; }

        public Setmeal Setmeal { get; set; }

        /// <summary>
        /// 菜品id
        /// </summary>
        public long? DishId { get; set; }

        public Dish Dish { get; set; }

        /// <summary>
        /// 菜品名称 （冗余字段）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜品单价（冗余字段）
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 菜品份数
        /// </summary>
        public int? Copies { get; set; }
    }
}
