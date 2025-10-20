namespace SkyTakeOut.Core.DTO.Category
{
    public class CategoryPageQueryDTO
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 分类类型 1菜品分类  2套餐分类
        /// </summary>
        public int? Type { get; set; }
    }
}
