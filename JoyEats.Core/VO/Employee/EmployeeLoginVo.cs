namespace JoyEats.Core.VO.Employee
{
    public class EmployeeLoginVo
    {
        /// <summary>
        /// 主键值
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// JWT令牌
        /// </summary>
        public string Token { get; set; }
    }
}
