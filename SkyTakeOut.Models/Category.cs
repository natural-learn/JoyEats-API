namespace SkyTakeOut.Models
{
    /// <summary>
    /// 菜品及套餐分类
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// 类型：1 菜品分类 2 套餐分类
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 分类状态 0:禁用，1:启用
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 导航属性，一个分类下有多个菜品
        /// </summary>
        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();

        /// <summary>
        /// 导航属性：一个分类下有多个套餐
        /// </summary>
        public ICollection<Setmeal> Setmeals { get; set; } = new List<Setmeal>();
    }
}
