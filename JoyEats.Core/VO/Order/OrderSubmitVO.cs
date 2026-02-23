namespace JoyEats.Core.VO.Order
{
    public class OrderSubmitVO
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
    }
}
