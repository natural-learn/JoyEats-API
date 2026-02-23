namespace JoyEats.Models
{
    /// <summary>
    /// 订单明细表
    /// </summary>
    public class OrderDetail
    {
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public long OrderId { get; set; }

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
        /// 导航属性，当前订单项所属的订单
        /// </summary>
        public Orders Orders { get; set; }
    }
}
