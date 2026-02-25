namespace JoyEats.Core.DTO.Order
{
    public class OrdersCancelDTO
    {
        public long Id { get; set; }

        /// <summary>
        /// 订单取消原因
        /// </summary>
        public string CancelReason { get; set; }
    }
}
