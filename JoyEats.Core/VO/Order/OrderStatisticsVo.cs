namespace JoyEats.Core.VO.Order
{
    public class OrderStatisticsVo
    {
        /// <summary>
        /// 待接单数量
        /// </summary>
        public int ToBeConfirmed { get; set; }

        /// <summary>
        /// 待派送数量
        /// </summary>
        public int Confirmed { get; set; }

        /// <summary>
        /// 派送中数量
        /// </summary>
        public int DeliveryInProgress { get; set; }
    }
}
