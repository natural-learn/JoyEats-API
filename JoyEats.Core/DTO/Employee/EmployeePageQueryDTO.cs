namespace JoyEats.Core.DTO.Employee
{
    public class EmployeePageQueryDTO
    {
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize { get; set; }
    }
}
