namespace SkyTakeOut.Core.DTO.Setmeal
{
    public class SetmealPageQueryDTO
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string? Name { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// 状态 0表示禁用 1表示启用
        /// </summary>
        public int? Status { get; set; }
    }
}
