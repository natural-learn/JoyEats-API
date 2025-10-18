namespace SkyTakeOut.Models
{
    public class Orders
    {
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 订单状态 1待付款 2待接单 3已接单 4派送中 5已完成 6已取消 7退款
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 下单用户
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 地址id
        /// </summary>
        public long AddressBookId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 结账时间
        /// </summary>
        public DateTime? CheckoutTime { get; set; }

        /// <summary>
        /// 支付方式 1微信,2支付宝
        /// </summary>
        public int PayMethod { get; set; }

        /// <summary>
        /// 支付状态 0未支付 1已支付 2退款
        /// </summary>
        public byte PayStatus { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 订单取消原因
        /// </summary>
        public string CancelReason { get; set; }

        /// <summary>
        /// 订单拒绝原因
        /// </summary>
        public string RejectionReason { get; set; }

        /// <summary>
        /// 订单取消时间
        /// </summary>
        public DateTime? CancelTime { get; set; }

        /// <summary>
        /// 预计送达时间
        /// </summary>
        public DateTime? EstimatedDeliveryTime { get; set; }

        /// <summary>
        /// 配送状态  1立即送出  0选择具体时间
        /// </summary>
        public bool DeliveryStatus { get; set; }

        /// <summary>
        /// 送达时间
        /// </summary>
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 打包费
        /// </summary>
        public int? PackAmount { get; set; }

        /// <summary>
        /// 餐具数量
        /// </summary>
        public int? TablewareNumber { get; set; }

        /// <summary>
        /// 餐具数量状态  1按餐量提供  0选择具体数量
        /// </summary>
        public bool TablewareStatus { get; set; }
    }

    public class OrderStatus
    {
        /**
        * 订单状态 1待付款 2待接单 3已接单 4派送中 5已完成 6已取消
        */
        public const int PENDING_PAYMENT = 1;
        public const int TO_BE_CONFIRMED = 2;
        public const int CONFIRMED = 3;
        public const int DELIVERY_IN_PROGRESS = 4;
        public const int COMPLETED = 5;
        public const int CANCELLED = 6;

        /**
        * 支付状态 0未支付 1已支付 2退款
        */
        public const int UN_PAID = 0;
        public const int PAID = 1;
        public const int REFUND = 2;
    }
}
