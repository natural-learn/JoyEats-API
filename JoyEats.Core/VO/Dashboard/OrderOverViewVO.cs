namespace JoyEats.Core.VO.Dashboard
{
    public class OrderOverViewVO
    {
        /// <summary>
        /// 待接单数量
        /// </summary>
        public int WaitingOrders { get; set; }

        /// <summary>
        /// 待派送数量
        /// </summary>
        public int DeliveredOrders { get; set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public int CompletedOrders { get; set; }

        /// <summary>
        /// 已取消数量
        /// </summary>
        public int CancelledOrders { get; set; }

        /// <summary>
        /// 全部订单
        /// </summary>
        public int AllOrders { get; set; }
    }
}
