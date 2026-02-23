namespace JoyEats.Core.VO.Dish
{
    public class DishItemVo
    {
        /// <summary>
        /// 菜品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 份数
        /// </summary>
        public int? Copies { get; set; }

        /// <summary>
        /// 菜品图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 菜品描述
        /// </summary>
        public string Description { get; set; }
    }
}
