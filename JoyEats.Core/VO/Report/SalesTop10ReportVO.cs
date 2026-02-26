namespace JoyEats.Core.VO.Report
{
    public class SalesTop10ReportVO
    {
        /// <summary>
        /// 商品名称列表，以逗号分隔，例如：鱼香肉丝,宫保鸡丁,水煮鱼
        /// </summary>
        public string NameList { get; set; }

        /// <summary>
        /// 销量列表，以逗号分隔，例如：260,215,200
        /// </summary>
        public string NumberList { get; set; }
    }
}
