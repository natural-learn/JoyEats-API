namespace JoyEats.Core.VO.Report
{
    public class TurnoverReportVO
    {
        /// <summary>
        /// 日期，以逗号分隔，例如：2022-10-01,2022-10-02,2022-10-03
        /// </summary>
        public string DateList { get; set; }

        /// <summary>
        /// 营业额，以逗号分隔，例如：406.0,1520.0,75.0
        /// </summary>
        public string TurnoverList { get; set; }
    }
}
