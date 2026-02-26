namespace JoyEats.Core.VO.Report
{
    public class OrderReportVO
    {
        /// <summary>
        /// 日期，以逗号分隔，例如：2022-10-01,2022-10-02,2022-10-03
        /// </summary>
        public string DateList { get; set; }

        /// <summary>
        /// 每日订单数，以逗号分隔，例如：260,210,215
        /// </summary>
        public string OrderCountList { get; set; }

        /// <summary>
        /// 每日有效订单数，以逗号分隔，例如：20,21,10
        /// </summary>
        public string ValidOrderCountList { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int TotalOrderCount { get; set; }

        /// <summary>
        /// 有效订单数
        /// </summary>
        public int ValidOrderCount { get; set; }

        /// <summary>
        /// 订单完成率
        /// </summary>
        public double OrderCompletionRate { get; set; }
    }
}
