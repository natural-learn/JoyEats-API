namespace SkyTakeOut.Models
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 菜品id
        /// </summary>
        public long? DishId { get; set; }

        /// <summary>
        /// 套餐id
        /// </summary>
        public long? SetmealId { get; set; }

        /// <summary>
        /// 口味
        /// </summary>
        public string? DishFlavor { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}
