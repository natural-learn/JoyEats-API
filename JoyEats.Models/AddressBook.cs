namespace JoyEats.Models
{
    /// <summary>
    /// 地址簿
    /// </summary>
    public class AddressBook
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 省级区划编号
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 省级名称
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 市级区划编号
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// 市级名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 区级区划编号
        /// </summary>
        public string DistrictCode { get; set; }

        /// <summary>
        /// 区级名称
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public int Label { get; set; }

        /// <summary>
        /// 默认 0 否 1是
        /// </summary>
        public byte IsDefault { get; set; }
    }
}
