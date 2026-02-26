namespace JoyEats.Core.VO.Dashboard
{
    public class BusinessDataVO
    {
        /// <summary>
        /// 营业额
        /// </summary>
        public decimal Turnover { get; set; }

        /// <summary>
        /// 有效订单数
        /// </summary>
        public int ValidOrderCount { get; set; }

        /// <summary>
        /// 订单完成率
        /// </summary>
        public double OrderCompletionRate { get; set; }

        /// <summary>
        /// 平均客单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 新增用户数
        /// </summary>
        public int NewUsers { get; set; }
    }
}
