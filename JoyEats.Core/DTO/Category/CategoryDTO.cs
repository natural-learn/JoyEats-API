namespace JoyEats.Core.DTO.Category
{
    public class CategoryDTO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 类型 1 菜品分类 2 套餐分类
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
