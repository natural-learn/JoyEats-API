namespace JoyEats.Core.VO.Report
{
    public class UserReportVO
    {
        /// <summary>
        /// 日期，以逗号分隔，例如：2022-10-01,2022-10-02,2022-10-03
        /// </summary>
        public string DateList { get; set; }

        /// <summary>
        /// 用户总量，以逗号分隔，例如：200,210,220
        /// </summary>
        public string TotalUserList { get; set; }

        /// <summary>
        /// 新增用户，以逗号分隔，例如：20,21,10
        /// </summary>
        public string NewUserList { get; set; }
    }
}
