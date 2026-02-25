namespace JoyEats.Core.DTO.Order
{
    public class OrdersRejectionDTO
    {
        public long Id { get; set; }

        /// <summary>
        /// 订单拒绝原因
        /// </summary>
        public string RejectionReason { get; set; }
    }
}
