namespace SkyTakeOut.Models
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class User
    {   
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 微信用户唯一标识
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; } = "";

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; } = "";

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; } = "";

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; } = "";
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
