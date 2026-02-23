namespace JoyEats.Core.DTO.Order
{
    public class OrdersSubmitDTO
    {
        /// <summary>
        /// 地址簿id
        /// </summary>
        public long AddressBookId { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public int PayMethod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 预计送达时间
        /// </summary>
        public DateTime EstimatedDeliveryTime { get; set; }

        /// <summary>
        /// 配送状态  1立即送出  0选择具体时间
        /// </summary>
        public int DeliveryStatus { get; set; }

        /// <summary>
        /// 餐具数量
        /// </summary>
        public int TablewareNumber { get; set; }

        /// <summary>
        /// 餐具数量状态  1按餐量提供  0选择具体数量
        /// </summary>
        public int TablewareStatus { get; set; }

        /// <summary>
        /// 打包费s
        /// </summary>
        public int PackAmount { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
