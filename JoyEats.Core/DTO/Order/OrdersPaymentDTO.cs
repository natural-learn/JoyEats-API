namespace JoyEats.Core.DTO.Order
{
    public class OrdersPaymentDTO
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public int PayMethod { get; set; }
    }
}
