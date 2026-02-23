namespace JoyEats.Models
{
    /// <summary>
    /// 菜品口味关系表
    /// </summary>
    public class DishFlavor
    {
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 菜品
        /// </summary>
        public long DishId { get; set; }

        /// <summary>
        /// 口味名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 口味数据list
        /// </summary>
        public string Value { get; set; }
    }
}
